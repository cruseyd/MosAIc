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
}
