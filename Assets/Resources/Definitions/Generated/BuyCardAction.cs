using System.Diagnostics;
using UnityEditor.SceneManagement;

public class BuyCardAction : GameAction {
    private CardIndex _card;
    public BuyCardAction(int agentID, GameActionArgs args, GameState state) : base(agentID, args, state)
    {
        Debug.Assert(args.cards.Count > 0);
        _card = args.cards[0];
    }
    protected override void Execute(GameState state)
    {
        int cost = state.GetCard(_card).GetStat(StatName.Level);
        AddEffect(new IncrementAgentStatEffect(0, StatName.PlayerFaith, -cost));
        AddEffect(new MoveCardEffect(_card, CardZoneName.PlayerDiscard));
    }
};