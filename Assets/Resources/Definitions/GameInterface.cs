using UnityEngine;
using UnityEngine.Scripting;

public class GameInterface : GameManager
{
    public override void DoubleClickCard(Card card)
    {
        var args = new GameActionArgs();
        if (state.phase == PhaseName.PlayerMain)
        {
            switch (card.zone.name)
            {
                case CardZoneName.Hand:
                    args.cards.Add(card.id);
                    instance.TakeAction(ActionName.PlayCard, state.currentPlayer, args);
                    break;
                case CardZoneName.Buy:
                    args.cards.Add(card.id);
                    instance.TakeAction(ActionName.BuyCard, state.currentPlayer, args);
                    break;
                default: break;
            }
        }
    }
    public override void HandleSpace()
    {
        Debug.Log("MainInterface::HandleSpace");
        var args = new GameActionArgs();
        switch (state.phase)
        {
            case PhaseName.Default:
                args.phase = PhaseName.GameStart;
                instance.TakeAction(ActionName.ChangePhase, state.currentPlayer, args);
                break;
            case PhaseName.GameStart:
                args.phase = PhaseName.PlayerMain;
                instance.TakeAction(ActionName.ChangePhase, state.currentPlayer, args);
                break;
            case PhaseName.PlayerMain:
                args.phase = PhaseName.EnemyMain;
                instance.TakeAction(ActionName.ChangePhase, state.currentPlayer, args);
                break;
            default: break;
        }
    }
}
