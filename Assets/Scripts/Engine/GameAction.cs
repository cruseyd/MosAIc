using System;
using System.Collections.Generic;
using UnityEngine;

public class GameActionArgs
{
    public List<int> players = new List<int>();
    public List<CardIndex> cards = new List<CardIndex>();
    public List<CardIndex> targets = new List<CardIndex>();
    public List<StatValuePair> stats = new List<StatValuePair>();
    public List<int> values = new List<int>();
    public List<CardZoneID> zones = new List<CardZoneID>();
    public PhaseName phase = PhaseName.Default;

    public GameActionArgs(){}
}

public class GameActionWithEffects
{
    public GameAction action;
    public List<GameEffect> effects = new List<GameEffect>();
    public GameState state;
}

public abstract class GameAction
{
    public static event Action<GameAction, GameState> onBeforeResolveAction;
    public static event Action<GameAction, GameState> onAfterResolveAction;
    private List<GameEffect> _effects = new List<GameEffect>();
    private GameState _initialState;
    private GameState _finalState;
    protected int player {get; private set;}
    public GameActionArgs args { get; private set; }
    public GameAction(int player, GameActionArgs args, GameState state)
    {
        this.player = player;
        _initialState = state;
        _finalState = new GameState(state);
        this.args = args;
    }
    protected void AddEffect(GameEffect effect)
    {
        _effects.Add(effect);
    }
    protected void PlayCard(Card card)
    {
        foreach (var ei in card.Play(args))
        {
            AddEffect(ei);
        }
    }
    protected abstract void Execute(GameState state);
    public GameActionWithEffects Resolve()
    {
        Execute(_finalState); //populates effects
        
        onBeforeResolveAction?.Invoke(this, _finalState);
        var actionWithEffects = new GameActionWithEffects();
        actionWithEffects.action = this;
        actionWithEffects.state = _finalState;
        foreach (var effect in _effects)
        {
            actionWithEffects.effects.AddRange(_finalState.Execute(effect));
        }
        onAfterResolveAction?.Invoke(this, _finalState);
        return actionWithEffects;
    }
}
