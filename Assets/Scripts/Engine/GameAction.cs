using System.Collections.Generic;
using UnityEngine;

public abstract class GameAction
{
    private List<GameEffect> _effects = new List<GameEffect>();
    private GameState _initialState;
    private GameState _finalState;
    protected int agentID {get; private set;}
    public GameAction(int agentID, GameState state)
    {
        this.agentID = agentID;
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
        Debug.Log("Resolving " + _effects.Count + " effects");
        foreach (var effect in _effects)
        {
            Debug.Log("Resolving effect: " + effect.GetType().ToString());
            
            _finalState.Execute(effect);
        }
        return _finalState;
    }
}

// public class StartGameAction : GameAction
// {
//     public StartGameAction(int agentID, GameState state) : base(agentID, state)
//     {
//     }

//     protected override void Execute(GameState state)
//     {
//         Deck deck = (Deck)state.GetCardZone(CardZoneName.Deck, agentID);
//         CardZone hand = state.GetCardZone(CardZoneName.Hand, agentID);
//         for (int ii = 0; ii < GameParams.Get(Parameter.StartingHandSize); ii++)
//         {
//             AddEffect(new DrawCardEffect(deck, hand));
//         }
//     }
// }
