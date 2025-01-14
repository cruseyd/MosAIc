using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class GameStateUI : Singleton<GameStateUI>
{
    private Canvas _mainCanvas;
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

    public static CardUI Spawn(Card card)
    {
        var cardUI = Spawn(card.data, card.agent);
        cardUI.SetVisible(false);
        cardUI.Define(card);
        cardUI.transform.SetParent(instance._mainCanvas.transform);
        Debug.Assert(!_cards.ContainsKey(card.id));
        _cards[card.id] = cardUI;
        return cardUI;
    }
    public static CardUI Spawn(CardData data, int agentID)
    {
        GameObject gameObject = null;
        if (data.prefab != null)
        {
            gameObject = Instantiate(data.prefab);
        } else {
            gameObject = Instantiate(ResourceManager.GetCardPrefab(data.type));
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
            _cards[cardUI.card.id] = null;
        }
        Destroy(cardUI.gameObject);
    }

    private static CardUI GetUI(Card card)
    {
        if (card == null) { return null; }
        if (_cards.ContainsKey(card.id))
        {
            return _cards[card.id];
        }
        return null;
    }

    private static CardZoneUI GetUI(CardZone zone)
    {
        if (zone == null) { return null; }
        var key = new Pair<CardZoneName, int>(zone.name, zone.agent);
        if (_zones.ContainsKey(key))
        {
            return _zones[key];
        }
        return null;
    }

    public static void Animate(GameActionWithEffects actionWithEffects)
    {
        instance.StartCoroutine(DoAnimate(actionWithEffects));
    }

    private static IEnumerator DoAnimate(GameActionWithEffects actionWithEffects)
    {
        foreach (var effect in actionWithEffects.effects)
        {
            yield return effect.Display();
        }
    }
    public void MoveCard(Card card, CardZone startZone, CardZone endZone)
    {
        animating = true;
        StartCoroutine(DoMoveCard(GetUI(card), GetUI(startZone), GetUI(endZone)));
    }

    private IEnumerator DoMoveCard(CardUI card, CardZoneUI start, CardZoneUI end)
    {
        yield return new WaitForSeconds(1.0f); //placeholder
        animating = false;
    }
}
