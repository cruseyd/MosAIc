using UnityEngine;

public class ChangePhaseAction : GameAction {
    private PhaseName _newPhase;
    public ChangePhaseAction(int agentID, GameActionArgs args, GameState state) : base(agentID, args, state)
    {
        Debug.Assert(args.phase != PhaseName.Default);
        _newPhase = args.phase;
    }
    protected override void Execute(GameState state)
    {
        Debug.Log("ChangePhaseAction::Execute");
        PhaseName oldPhase = state.phase;
        ExitPhase(oldPhase, _newPhase, state);
        AddEffect(new ChangePhaseEffect(_newPhase));
        EnterPhase(oldPhase, _newPhase, state);
    }

    private void FillPlayerHand(GameState state)
    {
        CardZone hand = state.GetCardZone(CardZoneName.Hand);
        int playerVitality = state.GetAgent(0).GetStat(StatName.PlayerVitality);
        for (int ii = 0; ii < playerVitality; ii++)
        {
            AddEffect(new DrawCardEffect(CardZoneName.PlayerDeck, CardZoneName.Hand));
        }
    }
    private void CyclePlayerHand(GameState state)
    {
        CardZone hand = state.GetCardZone(CardZoneName.Hand);
        foreach (var card in hand.Cards())
        {
            AddEffect(new MoveCardEffect(card, CardZoneName.PlayerDiscard));
        }
        FillPlayerHand(state);
    }
    private void CycleBuyZone(GameState state)
    {
        CardZone buy = state.GetCardZone(CardZoneName.Buy);
        AddEffect(new MoveCardEffect(buy.LastCard(), CardZoneName.BuyDiscard));
        AddEffect(new DrawCardEffect(CardZoneName.BuyDeck, CardZoneName.Buy, 0));
    }
    private void FillBuyZone(GameState state)
    {
        CardZone buy = state.GetCardZone(CardZoneName.Buy);
        for (int ii = 0; ii < 3; ii++)
        {
            AddEffect(new DrawCardEffect(CardZoneName.BuyDeck, CardZoneName.Buy, 0));
        }
    }

    private void ExitPhase(PhaseName oldPhase, PhaseName newPhase, GameState state)
    {
        Debug.Log($"ChangePhaseAction::ExitPhase {newPhase} from {oldPhase}");
        switch (oldPhase)
        {
            case PhaseName.GameStart:
                break;
            case PhaseName.PlayerMain:
                CyclePlayerHand(state);
                CycleBuyZone(state);
                break;
            default: break;
        }
    }

    private void EnterPhase(PhaseName oldPhase, PhaseName newPhase, GameState state)
    {
        Debug.Log($"ChangePhaseAction::EnterPhase {newPhase} from {oldPhase}");
        switch (newPhase)
        {
            case PhaseName.GameStart:
                FillPlayerHand(state);
                FillBuyZone(state);
                break;
            case PhaseName.PlayerMain:
            default: break;
        }
    }
};