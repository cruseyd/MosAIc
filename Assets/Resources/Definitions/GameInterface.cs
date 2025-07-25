using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Scripting;

public class GameInterface : GameManager
{
    public override void DoubleClickCard(Card card)
    {

        if (state.phase == PhaseName.PlayerMain)
        {
            var args = new GameActionArgs();
            if (Targeting())
            {
                bool success = targeter.add(card);
                Debug.Log("Tried to add target (success: " + success + ")");
                if (targeter.finished())
                {
                    Debug.Log("Targeter is finished. Playing the card");
                    args.cards.Add(targeter.source);
                    foreach (CardIndex ti in targeter.getTargets()) { args.targets.Add(ti); }
                    targeter = null;
                    instance.TakeAction(ActionName.PlayCard, state.currentPlayer, args);
                }
            }
            else {
                switch (card.zone.name)
                {
                    case CardZoneName.Hand:
                        if (card.GetTargets().Count > 0)
                        {
                            targeter = new Targeter(card.id, card.GetTargets());
                            Debug.Log("Begin Targeting");
                        }
                        else
                        {
                            args.cards.Add(card.id);
                            instance.TakeAction(ActionName.PlayCard, state.currentPlayer, args);
                        }
                        break;
                    case CardZoneName.Buy:
                        args.cards.Add(card.id);
                        instance.TakeAction(ActionName.BuyCard, state.currentPlayer, args);
                        break;
                    default: break;
                }
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
