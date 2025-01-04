using UnityEngine;

public class UnitTester : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TestGameState();
    }

    void TestGameState()
    {
        GameMode mode = GameParams.GameMode();
        Initializer initializer = (Initializer)mode.GetAssociatedClass();
        Debug.Log("Initializer is null: " + initializer == null);
        GameState state = initializer.Initialize();
    }
}
