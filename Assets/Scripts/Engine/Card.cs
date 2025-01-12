using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
    private Dictionary<StatName, Stat> stats;
    public CardZone zone { get; private set;}
    public int agent { get { return zone.agent; }}
    public int zonePosition
    {
        get {
            int position = zone.GetPosition(this);
            Debug.Assert(position >= 0);
            return position;
        }
    }
    public int id {get; private set; }
    public Card(CardData data_)
    {
        id = Random.Range(0,1000);
        data = data_;
        _visibleTo = new List<int>();
        orientation = CardOrientation.Up;
        stats = new Dictionary<StatName, Stat>();
        foreach (StatValuePair def in data_.baseStats)
        {
            stats.Add(def.stat, new Stat(def.stat, def.value));
        }
    }
    public Card(Card card)
    {
        id = card.id;
        data = card.data;
        _visibleTo = card._visibleTo;
        orientation = card.orientation;
        stats = card.stats;
    }
    public Stat GetStat(StatName statName)
    {  
        Debug.Assert(stats.ContainsKey(statName));
        return stats[statName];
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
        int newPosition = newZone.NumCards();
        Move(newZone, newPosition);
    }
    public void Move(int newPosition)
    {
        Debug.Assert(zone != null);
        Move(zone, newPosition);
    }
    public void Move(CardZone newZone, int newPosition)
    {
        zone?.Remove(this);
        newZone?.AddAtPosition(this, newPosition);
        zone = newZone;
    }
    
    public override string ToString()
    {
        string info = "Card | name: " + data.name + " | orient: " + orientation.ToString();
        foreach (StatName stat in stats.Keys)
        {
            info += " | " + stat.ToString() + " : " + stats[stat].value;
        }
        return info;
    }
}
