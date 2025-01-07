using UnityEditor;
using UnityEngine;

[System.Serializable]
public enum TestName
{
    GameState,
    MoveCard, 
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
            case TestName.MoveCard: InitMoveCard(); break;
        }
    }

    void Update()
    {
        switch (test)
        {
            case TestName.GameState: TestGameState(); break;
            case TestName.MoveCard: TestMoveCard(); break;
        }
    }
    void InitGameState()
    {
        GameMode mode = GameParams.GameMode();
        Initializer initializer = (Initializer)mode.GetAssociatedClass();
        GameState state = initializer.Initialize();
    }
    void TestGameState() {}

    void InitMoveCard() 
    {
        GameMode mode = GameParams.GameMode();
        Initializer initializer = (Initializer)mode.GetAssociatedClass();
        GameState state = initializer.Initialize();
        GameState.main = state;
    }
    void TestMoveCard()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var card = new Card(ResourceManager.GetRandomCardData());
            var hand = GameState.main.GetCardZone(CardZoneName.Hand,0);
            card.Move(hand, 0);
            SceneView.RepaintAll();
        } else if (Input.GetMouseButtonDown(1))
        {
            
            var hand = GameState.main.GetCardZone(CardZoneName.Hand, 0);
            var chars = GameState.main.GetCardZone(CardZoneName.Characters, 0);
            Card card = hand.cards[hand.NumCards()-1];
            Debug.Log($"Moving card {card.data.name} from hand to chars");
            Debug.Log($"There are {hand.NumCards()} cards in hand and {chars.NumCards()} cards in chars");
            card.Move(chars, 0);
            Debug.Log($"Now there are {hand.NumCards()} cards in hand and {chars.NumCards()} cards in chars");

            SceneView.RepaintAll();
        }
    }
}
