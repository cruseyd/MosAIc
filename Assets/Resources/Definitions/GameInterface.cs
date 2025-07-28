using System;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Scripting;

public class GameInterface : GameManager
{
    public override void DoubleClickCard(Card card)
    {
        if (Targeting())
        {
            AddTarget(card);
        }
        if (state.phase == PhaseName.PlayerMain)
        {
            var args = new GameActionArgs();
            if (!Targeting())
            {
                switch (card.zone.name)
                {
                    case CardZoneName.Hand:
                        if (card.GetTargets().Count > 0)
                        {
                            StartTargeting(card);
                        }
                        else
                        {
                            args.cards.Add(card.id);
                            TakeAction(ActionName.PlayCard, state.currentPlayer, args);
                        }
                        break;
                    case CardZoneName.Buy:
                        args.cards.Add(card.id);
                        TakeAction(ActionName.BuyCard, state.currentPlayer, args);
                        break;
                    default: break;
                }
            }

        }
    }
    public override void HandleSpace()
    {
        var args = new GameActionArgs();
        switch (state.phase)
        {
            case PhaseName.Default:
                args.phase = PhaseName.GameStart;
                TakeAction(ActionName.ChangePhase, state.currentPlayer, args);
                break;
            case PhaseName.GameStart:
                args.phase = PhaseName.PlayerMain;
                TakeAction(ActionName.ChangePhase, state.currentPlayer, args);
                break;
            case PhaseName.PlayerMain:
                args.phase = PhaseName.EnemyMain;
                TakeAction(ActionName.ChangePhase, state.currentPlayer, args);
                break;
            case PhaseName.EnemyMain:
                if (previewing != null)
                {
                    Card card = state.GetCard(previewing);
                    if (card.GetTargets().Count > 0)
                    {
                        StartTargeting(card);
                    }
                    else
                    {
                        args.cards.Add(card.id);
                        TakeAction(ActionName.PlayCard, state.currentPlayer, args);
                    }
                }
                else if (state.GetAgent(state.currentPlayer).GetStat(StatName.VillainCards) > 0)
                {
                    args.zones.Add(CardZoneName.VillainDeck);
                    TakeAction(ActionName.DrawAndPlay, state.currentPlayer, args);
                }
                else
                {
                    args.phase = PhaseName.PlayerMain;
                    TakeAction(ActionName.ChangePhase, state.currentPlayer, args);
                }
                break;
            default: break;
        }
    }

    public override void HandleEscape()
    {
        switch (state.phase)
        {
            case PhaseName.PlayerMain:
                EndTargeting();
                break;
            default: break;
        }
        
    }
}
