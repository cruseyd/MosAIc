using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Deck : CardZone
{
    public Deck() : base() {}
    public Deck(CardZoneName name_, int playerID_) : base(name_, playerID_) {}
    public Deck(Deck deck_) : base(deck_) {}
    public CardIndex Draw()
    {
        if (NumCards() == 0) { return null; }
        CardIndex topCard = GetCardAtIndex(Deserialize(0));
        return topCard;
    }
    public void Shuffle()
    {
        int N = NumCards();
        for (int ii = 0; ii < N; ii++)
        {
            int a = Random.Range(0,N);
            int b = Random.Range(0,N);
            if (a == b) { continue; }
            var tmp = _cards[a];
            _cards[a] = _cards[b];
            _cards[b] = tmp;
        }
    }
    public void InsertRandom(CardIndex card)
    {
        int randomPosition = Random.Range(0,NumCards());
        AddAtIndex(card, Deserialize(randomPosition));
    }
    protected override int Serialize(CardZoneIndex index)
    {
        return index.z;
    }
    protected override CardZoneIndex Deserialize(int position)
    {
        return new CardZoneIndex(0,0,position);
    }
}
