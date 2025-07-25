using System.Collections.Generic;
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

public class CardIndex{
    private static long _nextIndex = 1;
    public long index {get; private set;}
    public CardIndex()
    {
        index = _nextIndex;
        _nextIndex++;
    }
    public CardIndex(long index)
    {
        this.index = index;
    }
    public CardIndex(CardIndex index)
    {
        this.index = index.index;
    }
    public override bool Equals(object obj)
    {
        if (obj is CardIndex other) {
            return this.index == other.index;
        }
        return false;
    }

    public override int GetHashCode() {
        return index.GetHashCode(); // Tuple-based hash code generation
    }
}
public class Card
{
    public CardIndex id {get; private set; }
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
    public CardType type {
        get {
            return data.type;
        }
    }
    private Dictionary<StatName, int> stats = new Dictionary<StatName, int>();
    public CardZoneID zone { get; set;}
    protected CardAbility _ability;
    public Card(CardData data_)
    {
        id = new CardIndex();
        data = data_;
        _visibleTo = new List<int>();
        orientation = CardOrientation.Up;
        zone = null;
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
        zone = card.zone;
        stats = card.stats;
        _ability = data.GetAbility(this);
    }
    public List<GameEffect> Play(GameActionArgs args)
    {
        Debug.Assert(_ability != null);
        return _ability.Execute(args);
    }
    public List<CardFilter> GetTargets()
    {
        return _ability.targets;
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
        //TODO: Fix this when we need it. 

        // _visibleTo.Clear();
        // switch (visibility)
        // {
        //     case CardVisibility.Hidden: break;
        //     case CardVisibility.Owner: _visibleTo.Add(player); break;
        //     case CardVisibility.Visible:
        //         for (int id = 0; id < GameManager.state.NumAgents(); id++)
        //         {
        //             _visibleTo.Add(id);
        //         }
        //         break;
        // }
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
    public override string ToString()
    {
        string info = "Card | name: " + data.name;
        info += " | id: " + id.ToString();
        info += " | zone: " + zone;
        // foreach (StatName stat in stats.Keys)
        // {
        //     //info += " | " + stat.ToString() + " : " + stats[stat].value;
        // }
        return info;
    }
}
