using System.Collections.Generic;
using UnityEngine;

public class CardZone
{
    public CardZoneName name {get; private set;}
    public int player {get; private set;}
    public List<Card> cards { get; private set;}
    public CardZone(CardZoneName name_, int playerID_)
    {
        name = name_;
        player = playerID_;
        cards = new List<Card>();
    }

    public CardZone(CardZone zone)
    {
        name = zone.name;
        player = zone.player;
        cards = new List<Card>();
        foreach (Card card in zone.cards)
        {
            cards.Add(new Card(card));
        }
    }
}
