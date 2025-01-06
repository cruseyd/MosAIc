using System.Collections.Generic;
using UnityEngine;

public class CardZone
{
    public CardZoneName name {get; private set;}
    public int agent {get; private set;}
    public List<Card> cards { get; private set;}
    public CardZone(CardZoneName name_, int playerID_)
    {
        name = name_;
        agent = playerID_;
        cards = new List<Card>();
    }

    public CardZone(CardZone zone)
    {
        name = zone.name;
        agent = zone.agent;
        cards = new List<Card>();
        foreach (Card card in zone.cards)
        {
            cards.Add(new Card(card));
        }
    }

    public void AddCard(Card card)
    {
        AddCardAtPosition(card, cards.Count);
    }
    public void AddCardAtPosition(Card card, int position)
    {
        Debug.Assert(position > cards.Count);
        if (cards.Contains(card))
        {
            int prevPosition = GetPositionOfCard(card);
            if (position != prevPosition)
            {
                cards.RemoveAt(prevPosition);
                if (prevPosition < position) { position--; }
                cards.Insert(position, card);
            }
        } else {
            cards.Insert(position, card);
        }
    }
    public int GetPositionOfCard(Card card)
    {
        if (cards.Contains(card))
        {
            return cards.IndexOf(card);
        } else {
            return -1;
        }
    }
    public bool Contains(Card card)
    {
        return cards.Contains(card);
    }
    public void RemoveCard(Card card)
    {
        if (Contains(card))
        {
            cards.Remove(card);
        }
    }
}
