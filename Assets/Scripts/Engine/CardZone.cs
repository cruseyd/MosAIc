using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    public CardZoneName name {get; protected set;}
    public int agent {get; protected set;}
    protected List<Card> _cards = new List<Card>();
    public CardZone()
    {
        name = CardZoneName.Default;
        agent = -1;
    }
    public CardZone(CardZoneName name_, int playerID_)
    {
        name = name_;
        agent = playerID_;
    }
    public List<Card> Cards()
    {
        var  list = new List<Card>();
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
    public Card FirstCard()
    {
        return GetCardAtIndex(FirstIndex());
    }
    public Card LastCard()
    {
        return GetCardAtIndex(LastIndex()); 
    }
    public Card GetCardAtIndex(CardZoneIndex index)
    {
        if (NumCards() <= 0) { return null; }
        int position = Serialize(index);
        Debug.Assert(position < NumCards());
        Card card = _cards[position];
        return card;
    }
    public void Add(Card card)
    {
        AddAtIndex(card, NextIndex());
    }
    public void AddAtIndex(Card card, CardZoneIndex index)
    {
        int position = Serialize(index);

        if (Contains(card))
        {
            int prevPosition = Serialize(card.zoneIndex);
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
    public CardZoneIndex GetIndex(Card card)
    {
        Debug.Assert(Contains(card));
        int index = _cards.FindIndex((c) => c == card);
        return Deserialize(index);
    }
    public int GetPosition(Card card)
    {
        return Serialize(GetIndex(card));
    }
    public bool Contains(Card card)
    {
        return _cards.Contains(card);
    }
    public void Remove(Card card)
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
    public abstract CardZone Clone();
    protected void CloneCardsFrom(CardZone zone)
    {
        _cards.Clear();
        foreach (Card card in zone.Cards())
        {
            Card clone = new Card(card);
            clone.Move(this, card.zoneIndex);
        }
    }
    public static T Create<T>(CardZoneName name, int agent) where T : CardZone, new()
    {
        return (T)Activator.CreateInstance(typeof(T), name, agent);
    }
    public int NumCards() { return _cards.Count; }
    public int Compare(Card a, Card b)
    {
        Debug.Log("this zone is: " + name);
        Debug.Log("a is in zone " + a.zone.name + " | b is in zone " + b.zone.name);
        Debug.Log("a in zone? " + Contains(a));
        Debug.Log("b in zone? " + Contains(b));
        Debug.Log("a and b in same zone? " + (a.zone == b.zone));
        Debug.Assert(Contains(a) && Contains(b));
        return Serialize(a.zoneIndex) - Serialize(b.zoneIndex);
    }
    public override string ToString()
    {
        string info = "Card Zone | type: " + name.ToString() + " | agent: " + agent + " | numCards: " + NumCards();
        return info;
    }
}
