using System;
using UnityEngine;
public class MainInitializer : Initializer{

    // Initialize a GameState
    public override GameState Initialize()
    {
        GameState state = new GameState();

        state.AddAgent(AgentType.Player, 0);
        state.AddAgent(AgentType.Enemy, 1);

        foreach (CardZoneName zoneName in Enum.GetValues(typeof (CardZoneName)))
        {
            if (zoneName.ToString().Contains("Deck"))
            {
                state.AddDeck(zoneName, 0);
            } else {
                state.AddCardZone<LinearCardZone>(zoneName);
            }
        }

        state.activeAgentID = 0;
        return state;
    }
}