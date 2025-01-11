using UnityEditor;
using UnityEngine;

[System.Serializable]
public enum TestName
{
    GameState,
    MoveCard, 
    TestDeck,
    GameAction,
}

public class UnitTester : MonoBehaviour
{
    [SerializeField] private TestName test;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void Update()
    {
        switch (test)
        {
            case TestName.GameState: TestGameState(); break;
            case TestName.MoveCard: TestMoveCard(); break;
            case TestName.TestDeck: TestShuffleDeck(); break;
            case TestName.GameAction: TestGameAction(); break;
        }
    }
    void TestGameState() {}
    void TestMoveCard()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var card = new Card(ResourceManager.GetRandomCardData());
            var hand = GameManager.state.GetCardZone(CardZoneName.Hand,0);
            card.Move(hand, 0);
        } else if (Input.GetMouseButtonDown(1))
        {
            
            var hand = GameManager.state.GetCardZone(CardZoneName.Hand, 0);
            var chars = GameManager.state.GetCardZone(CardZoneName.Characters, 0);
            Card card = hand.GetLastCard();
            card.Move(chars, 0);
        }
    }
    void TestShuffleDeck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var card = new Card(ResourceManager.GetRandomCardData());
            Deck deck = GameManager.state.GetCardZone(CardZoneName.Deck, 0) as Deck;
            int n0 = deck.NumCards();
            deck.InsertRandom(card);
            int n1 = deck.NumCards();
            Debug.Assert(n1 - n0 == 1);

        }
        if (Input.GetMouseButtonDown(1))
        {
            Deck deck = GameManager.state.GetCardZone(CardZoneName.Deck, 0) as Deck;
            deck.Shuffle();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Deck deck = GameManager.state.GetCardZone(CardZoneName.Deck, 0) as Deck;
            var hand = GameManager.state.GetCardZone(CardZoneName.Hand,0);
            int nd0 = deck.NumCards();
            int nh0 = hand.NumCards();
            Card topCard = deck.GetFirstCard();
            Card drawn = deck.Draw(hand, 0);
            int nd1 = deck.NumCards();
            int nh1 = hand.NumCards();
            if (nd0 == 0)
            {
                Debug.Assert(drawn == null);
                Debug.Assert(nh1 == nh0);
            } else {
                Debug.Assert(drawn == topCard);
                Debug.Assert(nh1 - nh0 == 1);
                Debug.Assert(nd0 - nd1 == 1);
            }
            Debug.Log("TEST PASSED");
        }
    }
    void TestGameAction()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.TakeAction(ActionName.StartGame, 0);
        }
    }
}
