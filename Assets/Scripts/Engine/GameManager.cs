using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState state {get; private set; }
    void Awake()
    {
        GameMode mode = GameParams.GameMode();
        Initializer initializer = (Initializer)mode.GetAssociatedClass();
        state = initializer.Initialize();
    }
}
