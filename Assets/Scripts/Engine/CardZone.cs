using System.Collections.Generic;
using UnityEngine;

public class CardZone
{
    public CardZoneName name {get; private set;}
    public int agent {get; private set;}
    private List<Card> _cards;
    public CardZone(CardZoneName name_, int playerID_)
    {
        name = name_;
        agent = playerID_;
        _cards = new List<Card>();
    }

    public CardZone(CardZone zone)
    {
        name = zone.name;
        agent = zone.agent;
        _cards = new List<Card>();
        foreach (Card card in zone._cards)
        {
            _cards.Add(new Card(card));
        }
    }

    public List<Card> Cards()
    {
        var  list = new List<Card>();
        list.AddRange(_cards);
        return list;
    }
    public Card GetCardAtPosition(int position)
    {
        Debug.Assert(position < NumCards());
        return _cards[position];
    }
    public Card GetFirstCard() { return GetCardAtPosition(0); }
    public Card GetLastCard() { return GetCardAtPosition(NumCards()-1); }
    public void Add(Card card)
    {
        AddAtPosition(card, _cards.Count);
    }
    public void AddAtPosition(Card card, int position)
    {
        Debug.Assert(position <= _cards.Count);
        if (Contains(card))
        {
            int prevPosition = GetPosition(card);
            if (position != prevPosition)
            {
                _cards.RemoveAt(prevPosition);
                if (prevPosition < position) { position--; }
                _cards.Insert(position, card);
            }
        } else {
            _cards.Insert(position, card);
        }
    }
    public int GetPosition(Card card)
    {
        if (_cards.Contains(card))
        {
            return _cards.IndexOf(card);
        } else {
            return -1;
        }
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
    public int NumCards() { return _cards.Count; }
    public override string ToString()
    {
        string info = "Card Zone | type: " + name.ToString() + " | agent: " + agent + " | numCards: " + NumCards();
        return info;
    }
}
