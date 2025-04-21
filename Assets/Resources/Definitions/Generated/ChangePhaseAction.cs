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

    private void ExitPhase(PhaseName oldPhase, PhaseName newPhase, GameState state)
    {
        Debug.Log($"ChangePhaseAction::ExitPhase {newPhase} from {oldPhase}");
        switch (oldPhase)
        {
            case PhaseName.GameStart:
                break;
            case PhaseName.PlayerMain:
                
                // Cycle player hand
                CardZone hand = state.GetCardZone(CardZoneName.Hand);
                int playerVitality = state.GetAgent(0).GetStat(StatName.PlayerVitality);
                foreach( var card in hand.Cards())
                {
                    AddEffect(new MoveCardEffect(card, CardZoneName.PlayerDiscard));
                }
                for (int ii = 0; ii < playerVitality; ii++)
                {
                    AddEffect(new DrawCardEffect(CardZoneName.PlayerDeck, CardZoneName.Hand));
                }
                
                // Cycle buy zone
                CardZone buy = state.GetCardZone(CardZoneName.Buy);
                AddEffect(new MoveCardEffect(buy.LastCard(), CardZoneName.BuyDiscard));
                AddEffect(new DrawCardEffect(CardZoneName.BuyDeck, CardZoneName.Buy, 0));

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
                int playerVitality = state.GetAgent(0).GetStat(StatName.PlayerVitality);
                for (int ii = 0; ii < playerVitality; ii++)
                {
                    AddEffect(new DrawCardEffect(CardZoneName.PlayerDeck, CardZoneName.Hand));
                }
                
                CardZone buy = state.GetCardZone(CardZoneName.Buy);
                for (int ii = 0; ii < 3; ii++)
                {
                    AddEffect(new DrawCardEffect(CardZoneName.BuyDeck, CardZoneName.Buy, 0));
                }
                
                break;
            case PhaseName.PlayerMain:
            default: break;
        }
    }
};