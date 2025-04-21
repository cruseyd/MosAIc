public class PlayerEndPhase : Phase{

    // What happens at the start of this phase
    public override void Enter(Phase prevPhase, GameState state){}
    
    // What happens when this phase ends
    public override void Exit(Phase nextPhase, GameState state){}
    
    // What happens when the player manually ends this phase
    public override Phase Next(GameState state)
    {
        return base.Next(state);
    }
}