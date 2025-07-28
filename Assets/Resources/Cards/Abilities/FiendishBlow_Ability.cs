
using System.Collections.Generic;
public class FiendishBlow_Ability : CardAbility {

    public FiendishBlow_Ability(Card card) : base(card)
    {
        targets.Add(CardFilter.InZone(CardZoneName.Hand));
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
