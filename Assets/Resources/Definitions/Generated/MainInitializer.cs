using System;
using UnityEngine;
public class MainInitializer : Initializer{

    // Initialize a GameState
    public override GameState Initialize()
    {
        GameState state = new GameState();

        state.AddAgent(AgentType.Player, 0);
        state.GetAgentWithID(0).SetStat(StatName.Vitality, GameParams.Get(Parameter.PlayerBaseVitality));
        state.AddAgent(AgentType.Enemy, 1);

        state.AddCardZonesFromUI();

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