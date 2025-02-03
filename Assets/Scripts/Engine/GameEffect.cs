using System.Collections;
using UnityEngine;

public abstract class GameEffect
{
    private GameEffect _simultaneous = null;
    public GameEffect simultaneous { get { return _simultaneous; } }
    public abstract void Execute();
    public abstract IEnumerator Display();
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
    public override IEnumerator Display() { yield return null; }
}

public class DrawCardEffect : GameEffect
{
    public Deck deck;
    public CardZone toZone;
    public int toPosition;
    private Card drawnCard;
    public DrawCardEffect(Deck deck, CardZone toZone, int toPosition = 0)
    {
        this.deck = deck;
        this.toZone = toZone;
        this.toPosition = toPosition;
        drawnCard = null;
    }
    public override void Execute()
    {
        // before draw event
        drawnCard = deck.Draw(toZone, toPosition);
        // after draw event
    }

    public override IEnumerator Display()
    {
        Debug.Assert(drawnCard != null);
        float dt = 0.2f;
        CardZoneUI deckUI = GameStateUI.GetUI(deck);
        CardZoneUI toZoneUI = GameStateUI.GetUI(toZone);
        Debug.Assert(deckUI != null);
        Debug.Assert(toZone != null);
        CardUI cardUI = GameStateUI.Spawn(drawnCard, deckUI.transform);
        cardUI.SetVisible(true);
        yield return GameStateUI.DoMoveCard(cardUI, deckUI, toZoneUI, dt);
    }
}