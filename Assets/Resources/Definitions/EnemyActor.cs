using System.Collections.Generic;

public class EnemyActor : Actor
{
    public override List<GameAction> GetValidActions(GameState state, GameRules rules)
    {
        List<GameAction> actions = new List<GameAction>();

        return actions;
    }
    // Completely deterministic. Plays all actions in order. 
    public override GameAction ChooseAction(List<GameAction> validActions)
    {
        if (validActions.Count > 0)
        {
            return validActions[0];
        }
        return null;
    }
}