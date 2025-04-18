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
            //Card card = new Card(ResourceManager.GetRandomCardData());
            Card card = new Card(ResourceManager.GetCardData("Resolve"));
            state.AddCard(card, CardZoneName.PlayerDeck);
        }

        return state;
    }
    public override bool IsValid(ActionName action)
    {
        switch(action) {
            case ActionName.StartGame: return false;
            case ActionName.PlayCard: return true;
            default: return true;
        }
    }
}