using UnityEngine;

public class Initializer
{
    public virtual GameState Initialize()
    {
        return new GameState();
    }
}
