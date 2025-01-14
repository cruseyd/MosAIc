using System.Collections;
using UnityEngine;

public abstract class GameEffect
{
    private GameEffect _simultaneous = null;
    public GameEffect simultaneous { get { return _simultaneous; } }
    public abstract void Execute();
    public virtual IEnumerator Display()
    {
        yield return new WaitUntil(() => !GameStateUI.animating);
    }
    public void SimultaneousWith(GameEffect effect)
    {
        Debug.Assert(_simultaneous == null);
        _simultaneous = effect;
    }
}

public class MoveCardEffect : GameEffect
{
    public Card card;
    public CardZone zone;
    public CardZone prevZone;
    public MoveCardEffect(Card card, CardZone zone)
    {
        this.card = card;
        this.zone = zone;
        this.prevZone = card.zone;
    }
    public override void Execute()
    {
        // before card move event
        card.Move(zone);
        // after card move event
    }
    public override IEnumerator Display()
    {
        return base.Display();
    }
}

public class DrawCardEffect : GameEffect
{
    public Deck deck;
    public CardZone toZone;
    public int toPosition;
    public DrawCardEffect(Deck deck, CardZone toZone, int toPosition = 0)
    {
        this.deck = deck;
        this.toZone = toZone;
        this.toPosition = toPosition;
    }
    public override void Execute()
    {
        // before draw event
        Card card = deck.Draw(toZone, toPosition);
        // after draw event
    }
}