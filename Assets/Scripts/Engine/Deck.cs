using UnityEngine;
using UnityEngine.UIElements;

public class Deck : CardZone
{
    public Deck(CardZone zone) : base(zone)
    {
    }

    public Deck(CardZoneName name_, int playerID_) : base(name_, playerID_)
    {
    }
    public Card Draw(CardZone targetZone, int targetPosition)
    {
        if (NumCards() == 0) { return null; }
        Card topCard = GetFirstCard();
        Debug.Log("top card is in zone: " + topCard.zone.name.ToString());
        topCard.Move(targetZone, targetPosition);
        Debug.Log("top card is now in zone: " + topCard.zone.name.ToString());
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
        card.Move(this, randomPosition);
    }
    public void Insert(Card card, int position)
    {
        card.Move(this, position);
    }
}
