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
}
