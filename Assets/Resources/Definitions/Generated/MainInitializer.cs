using System;
using UnityEngine;
public class MainInitializer : Initializer{

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

        var playerDeck = state.GetDeck(CardZoneName.PlayerDeck);
        for (int ii = 0; ii < 20; ii++)
        {
            Card card = new Card(ResourceManager.GetRandomCardData());
            playerDeck.InsertRandom(card);
        }

        state.activeAgentID = 0;
        return state;
    }
}