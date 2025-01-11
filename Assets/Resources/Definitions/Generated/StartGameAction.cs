//using System.Diagnostics;
using UnityEngine;

public class StartGameAction : GameAction {
    public StartGameAction(int agentID, GameActionArgs args, GameState state) : base(agentID, args, state){}
    protected override void Execute(GameState state)
    {
        var deck = state.GetDeck(CardZoneName.Deck, agentID);
        CardZone hand = state.GetCardZone(CardZoneName.Hand, agentID);
        for (int ii = 0; ii < GameParams.Get(Parameter.StartingHandSize); ii++)
        {
            AddEffect(new DrawCardEffect(deck, hand));
        }
    }
};