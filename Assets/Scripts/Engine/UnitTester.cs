using UnityEditor;
using UnityEngine;

[System.Serializable]
public enum TestName
{
    GameState,
    MoveCard, 
    ShuffleDeck,
}

public class UnitTester : MonoBehaviour
{
    [SerializeField] private TestName test;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitGameState();
    }

    void Update()
    {
        switch (test)
        {
            case TestName.GameState: TestGameState(); break;
            case TestName.MoveCard: TestMoveCard(); break;
            case TestName.ShuffleDeck: TestShuffleDeck(); break;
        }
    }
    void InitGameState()
    {
        GameMode mode = GameParams.GameMode();
        Initializer initializer = (Initializer)mode.GetAssociatedClass();
        GameState state = initializer.Initialize();
        GameState.main = state;
    }
    void TestGameState() {}
    void TestMoveCard()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var card = new Card(ResourceManager.GetRandomCardData());
            var hand = GameState.main.GetCardZone(CardZoneName.Hand,0);
            card.Move(hand, 0);
        } else if (Input.GetMouseButtonDown(1))
        {
            
            var hand = GameState.main.GetCardZone(CardZoneName.Hand, 0);
            var chars = GameState.main.GetCardZone(CardZoneName.Characters, 0);
            Card card = hand.GetLastCard();
            card.Move(chars, 0);
        }
    }
    void TestShuffleDeck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var card = new Card(ResourceManager.GetRandomCardData());
            Deck deck = GameState.main.GetCardZone(CardZoneName.Deck, 0) as Deck;
            deck.InsertRandom(card);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Deck deck = GameState.main.GetCardZone(CardZoneName.Deck, 0) as Deck;
            deck.Shuffle();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Deck deck = GameState.main.GetCardZone(CardZoneName.Deck, 0) as Deck;
            var hand = GameState.main.GetCardZone(CardZoneName.Hand,0);
            deck.Draw(hand, 0);
        }
    }
}
