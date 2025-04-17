
using System.Collections.Generic;
public class Training_Ability : CardAbility {

    public Training_Ability(Card card) : base(card) {}

    public override List<GameEffect> Execute(GameActionArgs args)
    {
        var effects = new List<GameEffect>
        {
            // add effects here;
        };
        return effects;
    }
};
