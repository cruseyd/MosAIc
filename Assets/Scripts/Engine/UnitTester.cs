using System.Collections.Generic;
using Unity.VisualScripting;
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
            Dictionary<CardIndex, int> prevPositions = new Dictionary<CardIndex, int>();
            foreach (CardIndex ci in hand.Cards()) { prevPositions[ci] = hand.GetPosition(ci); }

            GameManager.state.MoveCard(card.id, CardZoneName.Hand, 0);
            
            int nh1 = hand.NumCards();
            Dictionary<CardIndex, int> postPositions = new Dictionary<CardIndex, int>();
            foreach (CardIndex ci in hand.Cards()) { prevPositions[ci] = hand.GetPosition(ci); }
            
            Debug.Assert(nh1 - nh0 == 1);
            foreach (var (ci, pi) in postPositions)
            {
                if (!prevPositions.ContainsKey(ci)) { Debug.Assert(ci == card.id); }
                else {
                    Debug.Assert(pi - prevPositions[ci] == 1);
                }
            }
        } else if (Input.GetMouseButtonDown(1))
        {
            var hand = GameManager.state.GetCardZone(CardZoneName.Hand);
            var inPlay = GameManager.state.GetCardZone(CardZoneName.InPlay);
            CardIndex card = hand.FirstCard();

            int nh0 = hand.NumCards();
            int nc0 = inPlay.NumCards();
            Dictionary<CardIndex, int> prevHandPositions = new Dictionary<CardIndex, int>();
            foreach (CardIndex ci in hand.Cards()) { prevHandPositions[ci] = hand.GetPosition(ci); }
            Dictionary<CardIndex, int> prevCharPositions = new Dictionary<CardIndex, int>();
            foreach (CardIndex ci in inPlay.Cards()) { prevCharPositions[ci] = hand.GetPosition(ci); }

            GameManager.state.MoveCard(card, CardZoneName.InPlay, 0);

            int nh1 = hand.NumCards();
            int nc1 = inPlay.NumCards();
            Dictionary<CardIndex, int> postHandPositions = new Dictionary<CardIndex, int>();
            foreach (CardIndex ci in hand.Cards()) { postHandPositions[ci] = hand.GetPosition(ci); }
            Dictionary<CardIndex, int> postCharPositions = new Dictionary<CardIndex, int>();
            foreach (CardIndex ci in inPlay.Cards()) { postCharPositions[ci] = hand.GetPosition(ci); }

            if (nh0 > 0)
            {
                Debug.Assert(nh1 - nh0 == -1);
                Debug.Assert(nc1 - nc0 == 1);
                foreach ( var (ci, pi) in prevHandPositions)
                {
                    if (!postHandPositions.ContainsKey(ci)) { Debug.Assert(ci == card); }
                    else {
                        Debug.Assert(pi - postHandPositions[ci] == 1);
                    }
                }
                foreach ( var (ci, pi) in postCharPositions)
                {
                    if (!prevCharPositions.ContainsKey(ci)) { Debug.Assert(ci == card); }
                    else {
                        Debug.Assert(pi - prevCharPositions[ci] == 1);
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
            Deck deck = GameManager.state.GetCardZone(CardZoneName.PlayerDeck) as Deck;
            int n0 = deck.NumCards();
            GameManager.state.AddCard(card, CardZoneName.PlayerDeck);
            int n1 = deck.NumCards();
            Debug.Assert(n1 - n0 == 1);

        }
        if (Input.GetMouseButtonDown(1))
        {
            Deck deck = GameManager.state.GetCardZone(CardZoneName.PlayerDeck) as Deck;
            deck.Shuffle();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Deck deck = GameManager.state.GetCardZone(CardZoneName.PlayerDeck) as Deck;
            var hand = GameManager.state.GetCardZone(CardZoneName.Hand);
            int nd0 = deck.NumCards();
            int nh0 = hand.NumCards();
            CardIndex topCard = deck.FirstCard();
            Card drawn = GameManager.state.DrawCard(CardZoneName.PlayerDeck, CardZoneName.Hand);
            int nd1 = deck.NumCards();
            int nh1 = hand.NumCards();
            if (nd0 == 0)
            {
                Debug.Assert(drawn == null);
                Debug.Assert(nh1 == nh0);
            } else {
                Debug.Assert(drawn.id == topCard);
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
        args.cards.Add(card.id);
        GameManager.instance.TakeAction(ActionName.PlayCard, 0, args);
    }
}
