using UnityEngine;
using UnityEngine.UIElements;

public class Deck : CardZone
{
    public Deck() : base() {}
    public Deck(CardZoneName name_, int playerID_) : base(name_, playerID_) {}
    public Card Draw(CardZone targetZone, int targetPosition)
    {
        if (NumCards() == 0) { return null; }
        Card topCard = GetCardAtIndex(Deserialize(0));
        topCard?.Move(targetZone, Deserialize(targetPosition));
        return topCard;
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
        card.Move(this, Deserialize(randomPosition));
    }
    public void Insert(Card card, int position)
    {
        card.Move(this, Deserialize(position));
    }
    protected override int Serialize(CardZoneIndex index)
    {
        return index.z;
    }
    protected override CardZoneIndex Deserialize(int position)
    {
        return new CardZoneIndex(0,0,position);
    }
    public override CardZone Clone()
    {
        var clone = new Deck(name, agent);
        clone.CloneCardsFrom(this);
        return clone;
    }
}
