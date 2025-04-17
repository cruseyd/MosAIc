
using System.Collections.Generic;
public class Conviction_Ability : CardAbility {

    public Conviction_Ability(Card card) : base(card) {}

    public override List<GameEffect> Execute(GameActionArgs args)
    {
        var effects = new List<GameEffect>
        {
            // add effects here;
        };
        return effects;
    }
};
