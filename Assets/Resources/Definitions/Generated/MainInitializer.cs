using UnityEngine;
public class MainInitializer : Initializer{

    // Initialize a GameState
    public override GameState Initialize()
    {
        GameState state = new GameState();
        state.AddAgent(AgentType.Player,0);
        state.AddAgent(AgentType.Enemy,1);
        for (int id = 0; id < state.NumAgents(); id++)
        {
            state.AddCardZone(CardZoneName.Hand, id);
            state.AddCardZone(CardZoneName.Characters, id);
            state.AddDeck(CardZoneName.Deck, id);
        }
        state.activeAgentID = 0;
        state.phase = PhaseName.Ready;
        return state;
    }
}