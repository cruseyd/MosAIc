using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System;

public abstract class GameEffect
{
    // Effect Events
    public static event Action<PhaseName, PhaseName> onExitPhase;
    public static event Action<PhaseName, PhaseName> onEnterPhase;

    // Event Callers
    protected void OnExitPhase(PhaseName oldPhase, PhaseName newPhase) { onExitPhase?.Invoke(oldPhase, newPhase); }
    protected void OnEnterPhase(PhaseName oldPhase, PhaseName newPhase) { onEnterPhase?.Invoke(oldPhase, newPhase); }

    private GameEffect _simultaneous = null;
    public GameEffect simultaneous { get { return _simultaneous; } }
    public abstract void Execute(GameState state);
    public void SimultaneousWith(GameEffect effect)
    {
        Debug.Assert(_simultaneous == null);
        _simultaneous = effect;
    }
}

public class MoveCardEffect : GameEffect
{
    public CardIndex cardIndex { get; private set; }
    public CardZoneID toZoneID  { get; private set; }
    public CardZoneID prevZoneID  { get; private set; }
    public int toZonePosition  { get; private set; }
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
}
public class DrawCardEffect : GameEffect
{
    public CardZoneID deckID { get; private set; }
    public CardZoneID toZoneID { get; private set; }
    public int toPosition { get; private set; }
    public Card drawnCard { get; private set; }
    public bool drawFromEmptyDeck { get; private set; }
    public CardZoneID sourceZoneID { get; private set; }
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
        drawFromEmptyDeck = deck.NumCards() == 0;
        drawnCard = state.DrawCard(deckID, toZoneID, toPosition);
        // after draw event
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

    public override void Execute(GameState state)
    {
        state.IncrementAgentStat(_player, _stat, _delta);
    }
}
public class SetAgentStatEffect : GameEffect
{
    private int _player;
    private StatName _stat;
    private int _value;
    public SetAgentStatEffect(int player, StatName stat, int value)
    {
        _player = player;
        _stat = stat;
        _value = value;
    }

    public override void Execute(GameState state)
    {
        state.SetAgentStat(_player, _stat, _value);
    }
}
public class ChangePhaseEffect : GameEffect
{
    public PhaseName newPhase { get; private set; }
    public PhaseName prevPhase { get; private set; }
    public ChangePhaseEffect(PhaseName newPhase)
    {
        this.newPhase = newPhase;
    }

    public override void Execute(GameState state)
    {
        prevPhase = state.phase;
        OnExitPhase(state.phase, newPhase);
        state.ChangePhase(newPhase);
        OnEnterPhase(state.phase, newPhase);
    }
}

public class ChangeActivePlayerEffect : GameEffect
{
    public int prevPlayer { get; private set; }
    public int newPlayer { get; private set; }

    public ChangeActivePlayerEffect(int newPlayer)
    {
        this.newPlayer = newPlayer;
    }

    public override void Execute(GameState state)
    {
        prevPlayer = state.currentPlayer;
        state.currentPlayer = newPlayer;
    }
}