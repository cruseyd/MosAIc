using System.Collections.Generic;
using UnityEngine;

public struct GameActionArgs
{

}

public abstract class GameAction
{
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
    public GameState Resolve()
    {
        foreach (var effect in _effects)
        {
            _finalState.Execute(effect);
        }
        return _finalState;
    }
}
