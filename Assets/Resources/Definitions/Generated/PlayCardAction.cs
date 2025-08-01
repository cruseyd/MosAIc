
using System.Collections.Generic;
using UnityEngine;

public class PlayCardAction : GameAction {
    private CardIndex _playedCardIndex;
    private List<CardIndex> _targetedCardIndices = new List<CardIndex>();
    public PlayCardAction(int player, GameActionArgs args, GameState state) : base(player, args, state)
    {
        Debug.Assert(args.cards.Count >= 1);
        _playedCardIndex = args.cards[0];
        if (args.cards.Count > 1)
        {
            for (int ii = 1; ii < args.cards.Count; ii++)
            {
                _targetedCardIndices.Add(args.cards[ii]);
            }
        }
    }
    protected override void Execute(GameState state)
    {
        // Get game objects from state
        // Add GameEffect instances using this.AddEffect
        Card playedCard = state.GetCard(_playedCardIndex);
        this.PlayCard(playedCard);
        switch (playedCard.type)
        {
            case CardType.PlayerAction:
                AddEffect(new IncrementAgentStatEffect(this.player, StatName.PlayerValor, playedCard.GetStat(StatName.Valor)));
                AddEffect(new IncrementAgentStatEffect(this.player, StatName.PlayerFaith, playedCard.GetStat(StatName.Faith)));
                AddEffect(new IncrementAgentStatEffect(this.player, StatName.PlayerVitality, playedCard.GetStat(StatName.Vitality)));
                AddEffect(new MoveCardEffect(_playedCardIndex, CardZoneName.PlayerDiscard));
                break;
            case CardType.EnemyAction:
                AddEffect(new IncrementAgentStatEffect(this.player, StatName.VillainCards, -1));
                AddEffect(new MoveCardEffect(_playedCardIndex, CardZoneName.VillainDiscard));
                break;
            default:
                Debug.LogWarning($"PlayCard action not implemented for card type {playedCard.data.type}");
                break;
        }
    }
};