using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[System.Serializable]
public class CardZoneID {
    [SerializeField] public CardZoneName name;
    [SerializeField] public int player;
    public CardZoneID(CardZoneName name, int player)
    {
        this.name = name;
        this.player = player;
    }
    public static implicit operator CardZoneID(CardZoneName name)
    {
        return new CardZoneID(name, 0);
    }
    public override bool Equals(object obj)
    {
        if (obj is CardZoneID other) {
            return this.name == other.name && this.player == other.player;
        }
        return false;
    }

    public override int GetHashCode() {
        return (name, player).GetHashCode(); // Tuple-based hash code generation
    }
}
[System.Serializable]
public struct CardZoneIndex
{
    public int x { get { return index.x; }}
    public int y { get { return index.y; }}
    public int z { get { return index.z; }}
    public CardZoneIndex(int x, int y, int z)
    {
        index = new Vector3Int(x,y,z);
    }
    public CardZoneIndex(int x, int y)
    {
        index = new Vector3Int(x, y, 0);
    }
    public CardZoneIndex(int x)
    {
        index = new Vector3Int(x, 0, 0);
    }
    public int GetSerialValue(int width, int height)
    {
        return x + width*y + width*height*z;
    }
    [SerializeField] private Vector3Int index;
}
public abstract class CardZone
{
    public CardZoneID id {get; private set;}
    protected List<CardIndex> _cards = new List<CardIndex>();
    public CardZone()
    {
        id = new CardZoneID(CardZoneName.Default, 0);
    }
    public CardZone(CardZoneName name_, int player)
    {
        id = new CardZoneID(name_, player);
    }
    public CardZone(CardZone zone)
    {
        _cards = zone._cards;
        id = zone.id;
    }
    public List<CardIndex> Cards()
    {
        var  list = new List<CardIndex>();
        list.AddRange(_cards);
        return list;
    }
    public CardZoneIndex FirstIndex()
    {
        return Deserialize(0);
    }
    public CardZoneIndex LastIndex()
    {
        int index = _cards
            .Select((value, idx) => value != null ? idx : -1)
            .LastOrDefault(i => i != -1); 
        index = (index == -1) ? _cards.Count : index;
        return Deserialize(index);
    }
    public CardZoneIndex NextIndex()
    {
        int index = _cards
            .Select((value, idx) => value == null ? idx : -1)
            .FirstOrDefault(i => i != -1); 
        index = (index == -1) ? _cards.Count : index;
        return Deserialize(index);
    }
    public CardIndex FirstCard()
    {
        return GetCardAtIndex(FirstIndex());
    }
    public CardIndex LastCard()
    {
        return GetCardAtIndex(LastIndex()); 
    }
    public CardIndex GetCardAtIndex(CardZoneIndex index)
    {
        if (NumCards() <= 0) { return null; }
        int position = Serialize(index);
        Debug.Assert(position < NumCards());
        CardIndex card = _cards[position];
        return card;
    }
    public void Add(CardIndex card)
    {
        AddAtIndex(card, NextIndex());
    }
    public void AddAtPosition(CardIndex card, int position)
    {
        AddAtIndex(card, Deserialize(position));
    }
    public void AddAtIndex(CardIndex card, CardZoneIndex index)
    {
        int position = Serialize(index);

        if (Contains(card))
        {
            var prevIndex = GetIndex(card);
            int prevPosition = Serialize(prevIndex);
            if (position != prevPosition)
            {
                _cards.RemoveAt(prevPosition);
                if (prevPosition < position) { position--; }
                _cards.Insert(position, card);
            }
        }
        else
        {
            if (position >= NumCards())
            {
                _cards.Insert(position, card);
            } else {
                if (_cards[position] == null)
                {
                    _cards[position] = card;
                } else {
                    _cards.Insert(position, card);
                }
            }
        }
    }
    public CardZoneIndex GetIndex(CardIndex card)
    {
        Debug.Assert(Contains(card));
        int index = _cards.FindIndex((c) => c == card);
        return Deserialize(index);
    }
    public int GetPosition(CardIndex card)
    {
        return Serialize(GetIndex(card));
    }
    public bool Contains(CardIndex card)
    {
        return _cards.Contains(card);
    }
    public void Remove(CardIndex card)
    {
        if (Contains(card))
        {
            _cards.Remove(card);
        }
    }
    protected abstract int Serialize(CardZoneIndex index);
    protected abstract CardZoneIndex Deserialize(int position);
    public CardZoneIndex Increment(CardZoneIndex index, int delta)
    {
        return Deserialize(Serialize(index)+delta);
    }
    // public abstract CardZone Clone();
    // protected void CloneCardsFrom(CardZone zone)
    // {
    //     _cards.Clear();
    //     foreach (Card card in zone.Cards())
    //     {
    //         Card clone = new Card(card);
    //         clone.Move(this, card.zoneIndex);
    //     }
    // }
    public static T Create<T>(CardZoneID id) where T : CardZone, new()
    {
        return (T)Activator.CreateInstance(typeof(T), id.name, id.player);
    }
    public int NumCards() { return _cards.Count; }
    public int Compare(CardIndex a, CardIndex b)
    {
        Debug.Assert(Contains(a) && Contains(b));
        return Serialize(GetIndex(a)) - Serialize(GetIndex(b));
    }
    public override string ToString()
    {
        string info = "Card Zone | type: " + id.name.ToString() + " | agent: " + id.player + " | numCards: " + NumCards();
        return info;
    }
}
