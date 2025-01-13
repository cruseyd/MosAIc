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
    private static Dictionary<Pair<CardZoneName, int>, DeckUI> _decks
        = new Dictionary<Pair<CardZoneName, int>, DeckUI>();

    protected override void Awake()
    {
        base.Awake();
        foreach (var item in _mainCanvas.GetComponentsInChildren<CardZoneUI>())
        {
            var key = new Pair<CardZoneName, int>(item.zoneName, item.agentID);
            if (item.GetType() == typeof(DeckUI))
            {
                //_decks[key] = (DeckUI)item;
            } else {
                //_zones[key] = item;
            }
            
        }
    }
}
