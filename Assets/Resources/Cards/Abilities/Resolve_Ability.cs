
using System.Collections.Generic;
public class Resolve_Ability : CardAbility {

    public Resolve_Ability(Card card) : base(card) {}

    public override List<GameEffect> Execute(GameActionArgs args)
    {
        var effects = new List<GameEffect>
        {
            new DrawCardEffect(CardZoneName.PlayerDeck, CardZoneName.Hand)
        };
        return effects;
    }
};
