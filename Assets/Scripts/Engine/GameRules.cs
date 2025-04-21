using UnityEngine;

public abstract class GameRules
{
    public abstract GameState Initialize();
    public abstract bool IsValid(ActionName action, int player, GameActionArgs args, GameState state);
}
