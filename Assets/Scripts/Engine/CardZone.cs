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
}
