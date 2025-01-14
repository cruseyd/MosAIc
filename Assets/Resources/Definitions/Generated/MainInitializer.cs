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
            state.AddCardZone<LinearCardZone>(CardZoneName.Hand, id);
            state.AddCardZone<LinearCardZone>(CardZoneName.Characters, id);
            state.AddCardZone<Deck>(CardZoneName.Deck, id);
            var deck = state.GetDeck(CardZoneName.Deck, id);
            for (int ii = 0; ii < GameParams.Get(Parameter.MinDeckSize); ii++)
            {
                Card card = new Card(ResourceManager.GetRandomCardData());
                deck.InsertRandom(card);
            }
        }
        state.activeAgentID = 0;
        state.phase = PhaseName.Ready;
        return state;
    }
}