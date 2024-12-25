using UnityEngine;

public enum CardOrientation
{
    UP,
    RIGHT,
    DOWN,
    LEFT
}
public class Card
{
    public bool faceUp
    {
        get
        {
            return _faceUp;
        } set {
            _faceUp = value;
        }
    }
    private bool _faceUp;
    public CardOrientation orientation
    {
        get {
            return _orientation;
        } set {
            _orientation = value;
        }
    }
    private CardOrientation _orientation;
    public CardData data {get; private set;}

    public Card(CardData data_)
    {
        data = data_;
        faceUp = false;
        orientation = CardOrientation.UP;
    }

    public Card(Card card)
    {
        data = card.data;
        faceUp = card.faceUp;
        orientation = card.orientation;
    }
}
