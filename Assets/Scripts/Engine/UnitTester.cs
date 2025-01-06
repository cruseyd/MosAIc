using UnityEngine;

[System.Serializable]
public enum TestName
{
    GameState,
}

public class UnitTester : MonoBehaviour
{
    [SerializeField] private TestName test;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (test)
        {
            case TestName.GameState: InitGameState(); break;
        }
    }

    void Update()
    {
        switch (test)
        {
            case TestName.GameState: TestGameState(); break;
        }
    }
    void InitGameState()
    {
        GameMode mode = GameParams.GameMode();
        Initializer initializer = (Initializer)mode.GetAssociatedClass();
        GameState state = initializer.Initialize();
    }
    void TestGameState() {}

    void InitMoveCard() {}
    void TestMoveCard() {}
}
