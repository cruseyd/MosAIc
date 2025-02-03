using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;



public class GameStateUI : Singleton<GameStateUI>
{
    [SerializeField] private Canvas _mainCanvas;
    private static Dictionary<long, CardUI> _cards
        = new Dictionary<long, CardUI>();
    private static Dictionary<Pair<CardZoneName, int>, CardZoneUI> _zones
        = new Dictionary<Pair<CardZoneName, int>, CardZoneUI>();

    public static bool animating { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        foreach (var zi in _mainCanvas.GetComponentsInChildren<CardZoneUI>())
        {
            var key = new Pair<CardZoneName, int>(zi.zoneName, zi.agentID);
            _zones[key] = zi;
        }
    }

    public static void BindState(GameState state)
    {
        foreach (var item in _zones.Keys)
        {
            _zones[item].zone = state.GetCardZone(item.first, item.second);
        }
    }

    public static CardUI Spawn(CardData data, CardZoneName zoneName, int agentID)
    {
        Transform spawnPoint = GetUI(zoneName, agentID).transform;
        return Spawn(data, agentID, spawnPoint);
    }

    public static CardUI Spawn(Card card, Transform spawnPoint)
    {
        var cardUI = Spawn(card.data, card.agent, spawnPoint);
        cardUI.SetVisible(false);
        cardUI.Define(card);
        cardUI.transform.SetParent(instance._mainCanvas.transform);
        _cards[card.id] = cardUI;
        return cardUI;
    }
    public static CardUI Spawn(CardData data, int agentID, Transform spawnPoint)
    {
        GameObject gameObject = null;
        if (data.prefab != null)
        {
            gameObject = Instantiate(data.prefab, spawnPoint);
        } else {
            gameObject = Instantiate(ResourceManager.GetCardPrefab(data.type), spawnPoint);
        }
        var cardUI = gameObject.GetComponent<CardUI>();
        cardUI.SetVisible(true);
        Debug.Assert(cardUI != null);
        cardUI.Define(data, agentID);
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

    public static CardUI GetUI(Card card)
    {
        if (card == null) { return null; }
        if (_cards.ContainsKey(card.id))
        {
            return _cards[card.id];
        }
        return null;
    }

    public static CardZoneUI GetUI(CardZone zone)
    {
        Debug.Assert(zone != null);
        return GetUI(zone.name, zone.agent);
    }
    public static CardZoneUI GetUI(CardZoneName zoneName, int agentID)
    {
        var key = new Pair<CardZoneName, int>(zoneName, agentID);
        if (_zones.ContainsKey(key))
        {
            return _zones[key];
        }
        return null;
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
        foreach (var effect in actionWithEffects.effects)
        {
            Debug.Assert(effect != null);
            yield return effect.Display();
        }
    }
    public static void MoveCard(Card card, CardZone startZone, CardZone endZone, float duration)
    {
        animating = true;
        instance.StartCoroutine(DoMoveCard(GetUI(card), GetUI(startZone), GetUI(endZone), duration));
    }

    public static IEnumerator DoMoveCard(CardUI card, CardZoneUI start, CardZoneUI end, float duration)
    {
        Debug.Assert(end != null);
        card.transform.SetParent(end.transform);
        start?.StartCoroutine(start.DoOrganize(duration));
        yield return end.DoOrganize(duration);
        animating = false;
    }

}
