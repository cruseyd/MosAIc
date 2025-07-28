public class DrawAndPlayAction : GameAction {
    public DrawAndPlayAction(int agentID, GameActionArgs args, GameState state) : base(agentID, args, state){}
    protected override void Execute(GameState state)
    {
        AddEffect(new DrawCardEffect(args.zones[0], CardZoneName.Preview));
    }
};