
using System.Collections.Generic;
using UnityEngine;

public class PlayCardAction : GameAction {
    private Card _playedCard;
    private List<Card> _targetedCards = new List<Card>();
    public PlayCardAction(int agentID, GameActionArgs args, GameState state) : base(agentID, args, state)
    {
        Debug.Assert(args.cards.Count >= 1);
        Debug.Log("number of cards in args: " + args.cards.Count);
        _playedCard = args.cards[0];
        if (args.cards.Count > 1)
        {
            for (int ii = 1; ii < args.cards.Count; ii++)
            {
                _targetedCards.Add(args.cards[ii]);
            }
        }
    }
    protected override void Execute(GameState state)
    {
        // Get game objects from state
        // Add GameEffect instances using this.AddEffect
        switch (_playedCard.type)
        {
            case CardType.PlayerAction:
                Agent player = state.GetAgentWithID(0);
                CardZone playerDiscard = state.GetCardZone(CardZoneName.PlayerDiscard);
                // card ability
                this.AddEffect(new IncrementAgentStatEffect(player, StatName.PlayerValor, _playedCard.GetStat(StatName.Valor)));
                this.AddEffect(new IncrementAgentStatEffect(player, StatName.PlayerFaith, _playedCard.GetStat(StatName.Faith)));
                this.AddEffect(new IncrementAgentStatEffect(player, StatName.PlayerVitality, _playedCard.GetStat(StatName.Vitality)));
                this.AddEffect(new MoveCardEffect(_playedCard, playerDiscard));
                break;
            default:
                Debug.LogWarning($"PlayCard action not implemented for card type {_playedCard.data.type}");
                break;
        }
    }
};