using System;
using System.Collections.Generic;
using UnityEngine;

public class GameActionArgs
{
    public List<Agent> agents = new List<Agent>();
    public List<Card> cards = new List<Card>();
    public List<int> values = new List<int>();

    public GameActionArgs(){}
    // Create a new GameActionArgs object with objects bound to given state
    public GameActionArgs(GameActionArgs args, GameState state)
    {
        if (args == null) { return; }
        cards.Clear();
        foreach (Card card in args.cards)
        {
            Card newCard = state.GetCardWithID(card.id);
            Debug.Assert(newCard != null);
            cards.Add(newCard);
        }
        agents.Clear();
        foreach (Agent agent in args.agents)
        {
            Agent newAgent = state.GetAgentWithID(agent.ID);
            Debug.Assert(newAgent != null);
            agents.Add(newAgent);
        }
        values.Clear();
        values.AddRange(args.values);
    }
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
    protected int agentID {get; private set;}
    protected GameActionArgs args { get; private set; }
    public GameAction(int agentID, GameActionArgs args, GameState state)
    {
        this.agentID = agentID;
        _initialState = state;
        _finalState = new GameState(state);
        this.args = new GameActionArgs(args, _finalState);
    }
    protected void AddEffect(GameEffect effect)
    {
        _effects.Add(effect);
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
