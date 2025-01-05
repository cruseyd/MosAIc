using UnityEngine;

public abstract class GameEffect
{
    public abstract void Execute(GameState state);
    public abstract void Undo(GameState state);
}
