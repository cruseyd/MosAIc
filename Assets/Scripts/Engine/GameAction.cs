using System;
using System.Collections.Generic;
using UnityEngine;

public struct GameActionArgs
{

}

public class GameActionWithEffects
{
    public GameAction action;
    public List<GameEffect> effects;
    public GameState state;
}

public abstract class GameAction
{
    public static event Action<GameAction, GameState> onBeforeResolveAction;
    public static event Action<GameAction, GameState> onAfterResolveAction;
    private List<GameEffect> _effects = new List<GameEffect>();
    private GameState _initialState;
    private GameState _finalState;
    protected int agentID {get; private set;}
    protected GameActionArgs args { get; private set; }
    public GameAction(int agentID, GameActionArgs args, GameState state)
    {
        this.agentID = agentID;
        this.args = args;
        _initialState = state;
        _finalState = new GameState(state);
        Execute(_finalState);
    }
    protected void AddEffect(GameEffect effect)
    {
        _effects.Add(effect);
    }
    protected abstract void Execute(GameState state);
    public GameActionWithEffects Resolve()
    {
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
