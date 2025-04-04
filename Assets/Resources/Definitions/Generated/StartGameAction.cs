using UnityEngine;

public class StartGameAction : GameAction {
    public StartGameAction(int agentID, GameActionArgs args, GameState state) : base(agentID, args, state){}
    protected override void Execute(GameState state)
    {
        Deck playerDeck = state.GetDeck(CardZoneName.PlayerDeck);
        CardZone playerHand = state.GetCardZone(CardZoneName.Hand);
        for (int ii = 0; ii < GameParams.Get(Parameter.PlayerBaseVitality); ii++)
        {
            AddEffect(new DrawCardEffect(playerDeck, playerHand));
        }
    }
};