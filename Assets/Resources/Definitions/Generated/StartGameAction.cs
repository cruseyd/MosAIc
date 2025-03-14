public class StartGameAction : GameAction {
    public StartGameAction(int agentID, GameActionArgs args, GameState state) : base(agentID, args, state){}
    protected override void Execute(GameState state)
    {
        // Get game objects from state
        // Add GameEffect instances using this.AddEffect
    }
};