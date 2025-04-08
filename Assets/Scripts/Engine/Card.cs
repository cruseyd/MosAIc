using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public enum CardOrientation
{
    Up,
    Right,
    Down,
    Left
}
public enum CardVisibility
{
    Hidden,
    Owner,
    Visible
}
public class Card
{
    private static long _nextSpawnID = 0;
    public long id {get; private set; }
    private List<int> _visibleTo;
    public CardOrientation orientation
    {
        get {
            return _orientation;
        } set {
            _orientation = value;
        }
    }
    private CardOrientation _orientation;
    public CardData data {get; private set;}
    private Dictionary<StatName, int> stats = new Dictionary<StatName, int>();
    public CardZone zone { get; private set;}
    public int agent { get { return zone.agent; }}
    public CardZoneIndex zoneIndex
    {
        get {
            Debug.Assert(zone.Contains(this));
            return zone.GetIndex(this);
        }
    }
    public int zonePosition
    {
        get {
            Debug.Assert(zone.Contains(this));
            return zone.GetPosition(this);
        }
    }
    protected CardAbility _ability;
    public Card(CardData data_)
    {
        id = _nextSpawnID;
        _nextSpawnID++;
        if (_nextSpawnID == long.MaxValue)
        {
            _nextSpawnID = 0;
        }
        data = data_;
        _visibleTo = new List<int>();
        orientation = CardOrientation.Up;
        stats.Clear();
        foreach (StatValuePair def in data_.baseStats)
        {
            stats.Add(def.stat, def.value);
        }
        _ability = data_.GetAbility(this);
    }
    public Card(Card card)
    {
        id = card.id;
        data = card.data;
        _visibleTo = card._visibleTo;
        orientation = card.orientation;
        stats = card.stats;
    }
    public int GetStat(StatName statName)
    {  
        Debug.Assert(stats.ContainsKey(statName));
        return stats[statName];
    }
    public void SetStat(StatName statName, int value)
    {
        Debug.Assert(stats.ContainsKey(statName));
        stats[statName] = value;
    }
    public void IncrementStat(StatName statName, int delta)
    {
        Debug.Assert(stats.ContainsKey(statName));
        stats[statName] = stats[statName] + delta;
    }
    public void SetVisibility(CardVisibility visibility)
    {
        _visibleTo.Clear();
        switch (visibility)
        {
            case CardVisibility.Hidden: break;
            case CardVisibility.Owner: _visibleTo.Add(agent); break;
            case CardVisibility.Visible:
                for (int id = 0; id < GameManager.state.NumAgents(); id++)
                {
                    _visibleTo.Add(id);
                }
                break;
        }
    }
    public void SetVisibility(int agent)
    {
        _visibleTo.Clear();
        _visibleTo.Add(agent);
    }
    public void ShareVisibilityWith(int agent)
    {
        if (!_visibleTo.Contains(agent))
        {
            _visibleTo.Add(agent);
        }
    }
    public bool VisibleTo(int agent)
    {
        return _visibleTo.Contains(agent);
    }
    public void Move(CardZone newZone)
    {
        if (newZone == zone) { return; }
        zone?.Remove(this);
        newZone?.Add(this);
        zone = newZone;
    }
    public void Move(CardZoneIndex index)
    {
        Debug.Assert(zone != null);
        Move(zone, index);
    }
    public void Move(CardZone newZone, CardZoneIndex index)
    {
        zone?.Remove(this);
        newZone?.AddAtIndex(this, index);
        zone = newZone;
    }
    public override string ToString()
    {
        string info = "Card | name: " + data.name;
        //info += " | orient: " + orientation.ToString();
        info += " | position: " + zonePosition.ToString();
        info += " | id: " + id.ToString();
        foreach (StatName stat in stats.Keys)
        {
            //info += " | " + stat.ToString() + " : " + stats[stat].value;
        }
        return info;
    }
}
