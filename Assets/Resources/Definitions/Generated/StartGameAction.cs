using UnityEngine;

public class StartGameAction : GameAction {
    public StartGameAction(int agentID, GameActionArgs args, GameState state) : base(agentID, args, state){}
    protected override void Execute(GameState state)
    {
        for (int ii = 0; ii < state.GetAgent(0).GetStat(StatName.PlayerVitality); ii++)
        {
            AddEffect(new DrawCardEffect(CardZoneName.PlayerDeck, CardZoneName.Hand));
        }
    }
};