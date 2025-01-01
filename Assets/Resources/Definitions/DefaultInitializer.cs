using UnityEngine;

public class DefaultInitializer : Initializer
{
    // Initialize a GameState
    public override GameState Initialize()
    {
        GameState state = new GameState();
        return new GameState();
    }
}