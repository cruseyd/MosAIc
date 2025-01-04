using UnityEngine;
public class MainInitializer : Initializer{

    // Initialize a GameState
    public override GameState Initialize()
    {
        GameState state = new GameState();
        state.AddAgent(new Agent(AgentType.Player, 0));
        state.AddAgent(new Agent(AgentType.Enemy, 1));
        state.currentPhase = PhaseName.GameStart;
        state.activeAgent = state.GetAgentWithID(0);
        return state;
    }
}