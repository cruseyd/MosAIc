using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public delegate IEnumerator EffectAnimationHandler(GameEffect effect, float speed);

public class GameStateUI : Singleton<GameStateUI>
{
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private TextMeshProUGUI _phaseText;
    private static Dictionary<CardIndex, CardUI> _cards
        = new Dictionary<CardIndex, CardUI>();
    private static Dictionary<CardZoneID, CardZoneUI> _zones
        = new Dictionary<CardZoneID, CardZoneUI>();
    private static Dictionary<int, AgentUI> _agents
        = new Dictionary<int, AgentUI>();

    private static Dictionary<Type, Delegate> _animationHandlers = new Dictionary<Type, Delegate>();
    public float animationSpeed = 1.0f;
    public static bool animating { get; private set; }

    private void RegisterAnimationHandlers()
    {
        _animationHandlers[typeof(MoveCardEffect)] = (EffectAnimationHandler)DisplayEffect_MoveCard;
        _animationHandlers[typeof(DrawCardEffect)] = (EffectAnimationHandler)DisplayEffect_DrawCard;
        _animationHandlers[typeof(ChangePhaseEffect)] = (EffectAnimationHandler)DisplayEffect_ChangePhase;
    }
    protected override void Awake()
    {
        base.Awake();
        foreach (var ai in _mainCanvas.GetComponentsInChildren<AgentUI>())
        {
            _agents[ai.id] = ai;
        }
        foreach (var zi in _mainCanvas.GetComponentsInChildren<CardZoneUI>())
        {
            _zones[zi.id] = zi;
        }

        // set events
        RegisterAnimationHandlers();
        GameManager.onTakeAction += DisplayAction;
    }

    public static void BindState(GameState state)
    {
        foreach (var (player, agentUI) in _agents)
        {
            agentUI.agent = state.GetAgent(player);
        }
        foreach (var (id, zoneUI) in _zones)
        {
            zoneUI.zone = state.GetCardZone(id);
        }
        foreach (var (index, cardUI) in _cards)
        {
            cardUI.card = state.GetCard(index);
        }
    }

    public static CardUI Spawn(CardData data, CardZoneID zoneID)
    {
        Transform spawnPoint = GetUI(zoneID).transform;
        return Spawn(data, spawnPoint);
    }

    public static CardUI Spawn(Card card, Transform spawnPoint)
    {
        var cardUI = Spawn(card.data, spawnPoint);
        cardUI.SetVisible(false);
        cardUI.card = card;
        cardUI.id = card.id.index;
        _cards[card.id] = cardUI;
        return cardUI;
    }
    public static CardUI Spawn(CardData data, Transform spawnPoint)
    {
        GameObject prefab;
        if (data.prefab != null)
        {
            prefab = Instantiate(data.prefab, spawnPoint);
        }
        else
        {
            prefab = Instantiate(ResourceManager.GetCardPrefab(data.type), spawnPoint);
        }
        var cardUI = prefab.GetComponent<CardUI>();
        Debug.Assert(cardUI != null);
        cardUI.SetVisible(true);
        cardUI.Define(data);
        cardUI.transform.SetParent(instance._mainCanvas.transform);
        return cardUI;
    }

    public static void Destroy(CardUI cardUI)
    {
        if (cardUI.card != null)
        {
            Debug.Assert(_cards.ContainsKey(cardUI.card.id));
            _cards.Remove(cardUI.card.id);
        }
        Destroy(cardUI.gameObject);
    }

    public static CardUI GetUI(CardIndex cardID)
    {
        if (_cards.ContainsKey(cardID))
        {
            return _cards[cardID];
        }
        return null;
    }

    public static CardZoneUI GetUI(CardZoneID zoneID)
    {
        if (_zones.ContainsKey(zoneID))
        {
            return _zones[zoneID];
        }
        return null;
    }

    public static List<CardZoneUI> GetAllCardZoneUI()
    {
        List<CardZoneUI> zones = new List<CardZoneUI>();
        foreach (CardZoneUI ui in _zones.Values)
        {
            zones.Add(ui);
        }
        return zones;
    }
    public static void Animate(GameActionWithEffects actionWithEffects)
    {
        Debug.Assert(actionWithEffects != null);
        Debug.Assert(instance != null);
        BindState(actionWithEffects.state);
        instance.StartCoroutine(DoAnimate(actionWithEffects));
    }

    private static IEnumerator DoAnimate(GameActionWithEffects actionWithEffects)
    {
        animating = true;
        foreach (var effect in actionWithEffects.effects)
        {
            Debug.Assert(effect != null);
            yield return instance.DisplayEffect(effect, instance.animationSpeed);
        }
        animating = false;
    }
    public static void MoveCard(CardIndex cardID, CardZoneID startZoneID, CardZoneID endZoneID, float duration)
    {
        instance.StartCoroutine(DoMoveCard(GetUI(cardID), GetUI(startZoneID), GetUI(endZoneID), duration));
    }

    public static IEnumerator DoMoveCard(CardUI card, CardZoneUI start, CardZoneUI end, float duration)
    {
        Debug.Assert(end != null);
        animating = true;
        card.transform.SetParent(end.transform);
        start?.StartCoroutine(start.DoOrganize(duration));
        yield return end.DoOrganize(duration);
        animating = false;
    }

    private void DisplayAction(GameActionWithEffects action, GameState prevState, GameState postState)
    {

    }

    private IEnumerator DisplayEffect(GameEffect effect, float speed)
    {
        Type T = effect.GetType();
        if (_animationHandlers.TryGetValue(T, out Delegate handler))
        {
            yield return ((EffectAnimationHandler)handler)(effect, speed);
        }
        else
        {
            yield return null;
        }
    }


    // Individual Effect Logic
    private IEnumerator DisplayEffect_MoveCard(GameEffect e, float speed)
    {
        Debug.Assert(e is MoveCardEffect);
        var effect = (MoveCardEffect)e;

        Debug.Assert(effect.cardIndex != null);
        float dt = 0.2f / speed;
        CardZoneUI newZoneUI = GetUI(effect.toZoneID);
        CardZoneUI oldZoneUI = GetUI(effect.prevZoneID);
        CardUI cardUI = GetUI(effect.cardIndex);
        cardUI.SetVisible(true);
        yield return DoMoveCard(cardUI, oldZoneUI, newZoneUI, dt);
    }
    private IEnumerator DisplayEffect_DrawCard(GameEffect e, float speed)
    {
        Debug.Assert(e is DrawCardEffect);
        var effect = (DrawCardEffect)e;

        Debug.Assert(effect.drawnCard != null);
        float dt = 0.2f / speed;
        CardZoneUI deckUI = GetUI(effect.deckID);
        CardZoneUI toZoneUI = GetUI(effect.toZoneID);
        CardUI cardUI = Spawn(effect.drawnCard, deckUI.transform);
        if (effect.drawFromEmptyDeck && effect.sourceZoneID.name != CardZoneName.Default)
        {
            foreach (CardUI ui in GetUI(effect.sourceZoneID).GetCards())
            {
                Destroy(ui);
            }
        }
        cardUI.SetVisible(true);
        yield return DoMoveCard(cardUI, deckUI, toZoneUI, dt);
    }
    private IEnumerator DisplayEffect_ChangePhase(GameEffect e, float speed)
    {
        Debug.Assert(e is ChangePhaseEffect);
        var effect = (ChangePhaseEffect)e;

        _phaseText.text = "Phase: " + effect.newPhase.ToString();
        yield return null;
    }
}
