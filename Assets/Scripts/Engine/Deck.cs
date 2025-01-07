using UnityEngine;

public class Deck : CardZone
{
    public Deck(CardZone zone) : base(zone)
    {
    }

    public Deck(CardZoneName name_, int playerID_) : base(name_, playerID_)
    {
    }

    public void Shuffle()
    {
        foreach (var card in Cards())
        {
            InsertRandom(card);
        }
    }
    public void InsertRandom(Card card)
    {
        int randomPosition = Random.Range(0,NumCards());
        AddAtPosition(card, randomPosition);
    }
    public void Insert(Card card, int position)
    {
        AddAtPosition(card, position);
    }
}
