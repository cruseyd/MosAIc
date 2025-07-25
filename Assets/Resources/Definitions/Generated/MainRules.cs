using System;
using UnityEngine;
public class MainRules : GameRules{

    // Initialize a GameState
    public override GameState Initialize()
    {
        GameState state = new GameState();
        
        state.AddCardZonesFromUI();

        AgentData playerData = ResourceManager.GetRandomAgentData(AgentType.Player);
        Agent player = new Agent(playerData, 0);
        state.AddAgent(player);
        
        AgentData enemyData = ResourceManager.GetRandomAgentData(AgentType.Enemy);
        Agent enemy = new Agent(enemyData, 1);
        state.AddAgent(enemy);

        for (int ii = 0; ii < 20; ii++)
        {
            Card card = new Card(ResourceManager.GetCardData("Mercy"));
            state.AddCard(card, CardZoneName.PlayerDeck);
        }
        for (int ii = 0; ii < 20; ii++)
        {
            Card card = new Card(ResourceManager.GetRandomCardData());
            state.AddCard(card, CardZoneName.BuyDeck);
        }

        return state;
    }
    public override bool IsValid(ActionName action, int player, GameActionArgs args, GameState state)
    {
        switch(action) {
            case ActionName.StartGame: return false;
            case ActionName.PlayCard: return state.currentPlayer == player;
            case ActionName.BuyCard:
                bool valid = state.currentPlayer == player;
                valid &= state.currentPlayer == 0;
                Debug.Assert(args.cards.Count > 0);
                Card buyCard = state.GetCard(args.cards[0]);
                valid &= buyCard.zone.name == CardZoneName.Buy;
                int cardCost = buyCard.GetStat(StatName.Level);
                int playerFaith = state.GetAgent(player).GetStat(StatName.PlayerFaith);
                valid &= playerFaith >= cardCost;
                return valid;
            default: return true;
        }
    }
}