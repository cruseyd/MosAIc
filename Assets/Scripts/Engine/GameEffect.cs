using UnityEngine;

public abstract class GameEffect
{
    private GameEffect _simultaneous = null;
    public GameEffect simultaneous { get { return _simultaneous; } }
    public abstract void Execute();
    public void SimultaneousWith(GameEffect effect)
    {
        Debug.Assert(_simultaneous == null);
        _simultaneous = effect;
    }
}

public class MoveCardEffect : GameEffect
{
    private Card _card;
    private CardZone _zone;
    public MoveCardEffect(Card card, CardZone zone)
    {
        _card = card;
        _zone = zone;
    }
    public override void Execute()
    {
        // before card move event
        _card.Move(_zone);
        // after card move event
    }
}

public class DrawCardEffect : GameEffect
{
    private Deck _deck;
    private CardZone _toZone;
    private int _toPosition;
    public DrawCardEffect(Deck deck, CardZone toZone, int toPosition = 0)
    {
        _deck = deck;
        _toZone = toZone;
        _toPosition = toPosition;
    }
    public override void Execute()
    {
        // before draw event
        Card card = _deck.Draw(_toZone, _toPosition);
        // after draw event
    }
}