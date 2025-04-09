using System.Collections.Generic;
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
        GameManager.onCardDoubleClick += PlayCard;
    }

    void Update()
    {
        switch (test)
        {
            case TestName.GameState: TestGameState(); break;
            case TestName.MoveCard: TestMoveCard(); break;
            case TestName.TestDeck: TestDeck(); break;
            case TestName.GameAction: TestGameAction(); break;
        }
    }
    void TestGameState() {}
    void TestMoveCard()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var card = new Card(ResourceManager.GetRandomCardData());
            var hand = GameManager.state.GetCardZone(CardZoneName.Hand);

            int nh0 = hand.NumCards();
            Dictionary<Card, int> prevPositions = new Dictionary<Card, int>();
            foreach (Card ci in hand.Cards()) { prevPositions[ci] = ci.zonePosition; }

            card.Move(hand, hand.FirstIndex());
            
            int nh1 = hand.NumCards();
            Dictionary<Card, int> postPositions = new Dictionary<Card, int>();
            foreach (Card ci in hand.Cards()) { prevPositions[ci] = ci.zonePosition; }
            
            Debug.Assert(nh1 - nh0 == 1);
            foreach (Card ci in postPositions.Keys)
            {
                if (!prevPositions.ContainsKey(ci)) { Debug.Assert(ci == card); }
                else {
                    Debug.Assert(postPositions[ci] - prevPositions[ci] == 1);
                }
            }
        } else if (Input.GetMouseButtonDown(1))
        {
            var hand = GameManager.state.GetCardZone(CardZoneName.Hand, 0);
            var chars = GameManager.state.GetCardZone(CardZoneName.InPlay, 0);
            Card card = hand.FirstCard();

            int nh0 = hand.NumCards();
            int nc0 = chars.NumCards();
            Dictionary<Card, int> prevHandPositions = new Dictionary<Card, int>();
            foreach (Card ci in hand.Cards()) { prevHandPositions[ci] = ci.zonePosition; }
            Dictionary<Card, int> prevCharPositions = new Dictionary<Card, int>();
            foreach (Card ci in chars.Cards()) { prevCharPositions[ci] = ci.zonePosition; }

            card.Move(chars, chars.FirstIndex());

            int nh1 = hand.NumCards();
            int nc1 = chars.NumCards();
            Dictionary<Card, int> postHandPositions = new Dictionary<Card, int>();
            foreach (Card ci in hand.Cards()) { postHandPositions[ci] = ci.zonePosition; }
            Dictionary<Card, int> postCharPositions = new Dictionary<Card, int>();
            foreach (Card ci in chars.Cards()) { postCharPositions[ci] = ci.zonePosition; }

            if (nh0 > 0)
            {
                Debug.Assert(nh1 - nh0 == -1);
                Debug.Assert(nc1 - nc0 == 1);
                foreach (Card ci in prevHandPositions.Keys)
                {
                    if (!postHandPositions.ContainsKey(ci)) { Debug.Assert(ci == card); }
                    else {
                        Debug.Assert(prevHandPositions[ci] - postHandPositions[ci] == 1);
                    }
                }
                foreach (Card ci in postCharPositions.Keys)
                {
                    if (!prevCharPositions.ContainsKey(ci)) { Debug.Assert(ci == card); }
                    else {
                        Debug.Assert(postCharPositions[ci] - prevCharPositions[ci] == 1);
                    }
                }
            }
        }
    }
    void TestDeck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var card = new Card(ResourceManager.GetRandomCardData());
            Deck deck = GameManager.state.GetCardZone(CardZoneName.PlayerDeck, 0) as Deck;
            int n0 = deck.NumCards();
            deck.InsertRandom(card);
            int n1 = deck.NumCards();
            Debug.Assert(n1 - n0 == 1);

        }
        if (Input.GetMouseButtonDown(1))
        {
            Deck deck = GameManager.state.GetCardZone(CardZoneName.PlayerDeck, 0) as Deck;
            deck.Shuffle();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Deck deck = GameManager.state.GetCardZone(CardZoneName.PlayerDeck, 0) as Deck;
            var hand = GameManager.state.GetCardZone(CardZoneName.Hand,0);
            int nd0 = deck.NumCards();
            int nh0 = hand.NumCards();
            Card topCard = deck.FirstCard();
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
        }
    }
    void TestGameAction()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.TakeAction(ActionName.StartGame, 0);
        }
    }
    void PlayCard(Card card)
    {
        GameActionArgs args = new GameActionArgs();
        args.cards.Add(card);
        GameManager.instance.TakeAction(ActionName.PlayCard, 0, args);
    }
    void TestCardUI()
    {
        
    }
}
