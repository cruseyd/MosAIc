
using System.Collections.Generic;
using System.Diagnostics;
public class Mercy_Ability : CardAbility {

    public Mercy_Ability(Card card) : base(card)
    {
        CardFilter filter = new CardFilter();
        filter.inZone = CardZoneName.Buy;
        targets.Add(filter);
    }

    public override List<GameEffect> Execute(GameActionArgs args)
    {
        var effects = new List<GameEffect>
        {
            new MoveCardEffect(args.targets[0], CardZoneName.PlayerDiscard)
        };
        return effects;
    }
};
