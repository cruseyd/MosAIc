
using System.Collections.Generic;
public class Discipline_Ability : CardAbility {

    public Discipline_Ability(Card card) : base(card) {}

    public override List<GameEffect> Execute(GameActionArgs args)
    {
        var effects = new List<GameEffect>
        {
            // add effects here;
        };
        return effects;
    }
};
