using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class GameEffect
{
    private GameEffect _simultaneous = null;
    public GameEffect simultaneous { get { return _simultaneous; } }
    public abstract void Execute(GameState state);
    public abstract IEnumerator Display(float speed = 1.0f);
    public void SimultaneousWith(GameEffect effect)
    {
        Debug.Assert(_simultaneous == null);
        _simultaneous = effect;
    }
}

public class MoveCardEffect : GameEffect
{
    private CardIndex cardIndex;
    private CardZoneID toZoneID;
    private CardZoneID prevZoneID;
    private int toZonePosition;
    public MoveCardEffect(CardIndex cardIndex, CardZoneID toZoneID, int toZonePosition = 0)
    {
        this.cardIndex = cardIndex;
        this.toZoneID = toZoneID;
        this.toZonePosition = toZonePosition;
    }
    public override void Execute(GameState state)
    {
        // before card move event
        prevZoneID = state.GetCard(cardIndex).zone;
        state.MoveCard(cardIndex, toZoneID, toZonePosition);
        // after card move event
    }
    public override IEnumerator Display(float speed = 1.0f)
    {
        Debug.Assert(cardIndex != null);
        float dt = 0.2f/speed;
        CardZoneUI newZoneUI = GameStateUI.GetUI(toZoneID);
        CardZoneUI oldZoneUI = GameStateUI.GetUI(prevZoneID);
        CardUI cardUI = GameStateUI.GetUI(cardIndex);
        cardUI.SetVisible(true);
        yield return GameStateUI.DoMoveCard(cardUI, oldZoneUI, newZoneUI, dt);
    }
}

public class DrawCardEffect : GameEffect
{
    public CardZoneID deckID;
    public CardZoneID toZoneID;
    public int toPosition;
    private Card drawnCard;
    private bool drawFromEmptyDeck = false;
    private CardZoneID sourceZoneID;
    public DrawCardEffect(CardZoneID deckID, CardZoneID toZoneID, int toPosition = 0)
    {
        this.deckID = deckID;
        this.toZoneID = toZoneID;
        this.toPosition = toPosition;
        drawnCard = null;
    }
    public override void Execute(GameState state)
    {
        // before draw event
        var deck = (Deck)state.GetCardZone(deckID);
        sourceZoneID = deck.sourceZone;
        drawFromEmptyDeck = (deck.NumCards() == 0);
        drawnCard = state.DrawCard(deckID, toZoneID, toPosition);
        // after draw event
    }

    public override IEnumerator Display(float speed = 1.0f)
    {
        Debug.Assert(drawnCard != null);
        float dt = 0.2f/speed;
        CardZoneUI deckUI = GameStateUI.GetUI(deckID);
        CardZoneUI toZoneUI = GameStateUI.GetUI(toZoneID);
        CardUI cardUI = GameStateUI.Spawn(drawnCard, deckUI.transform);
        if (drawFromEmptyDeck && sourceZoneID.name != CardZoneName.Default)
        {
            foreach (CardUI ui in GameStateUI.GetUI(sourceZoneID).GetCards())
            {
                ui.Delete();
            }
        }
        cardUI.SetVisible(true);
        yield return GameStateUI.DoMoveCard(cardUI, deckUI, toZoneUI, dt);
    }
}
public class IncrementAgentStatEffect : GameEffect
{
    private int _player;
    private StatName _stat;
    private int _delta;
    public IncrementAgentStatEffect(int player, StatName stat, int delta)
    {
        _player = player;
        _stat = stat;
        _delta = delta;
    }

    public override IEnumerator Display(float speed = 1.0f)
    {
        yield return null;
    }

    public override void Execute(GameState state)
    {
        state.IncrementAgentStat(_player, _stat, _delta);
    }
}
public class ChangePhaseEffect : GameEffect
{
    PhaseName _newPhase;
    public ChangePhaseEffect(PhaseName newPhase)
    {
        _newPhase = newPhase;
    }

    public override IEnumerator Display(float speed = 1.0f)
    {
        yield return null;
    }

    public override void Execute(GameState state)
    {
        // leave phase events
        // enter phase events
        state.ChangePhase(_newPhase);
    }
}