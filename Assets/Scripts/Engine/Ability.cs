using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public abstract List<GameEffect> Execute(GameActionArgs args);
}
public abstract class CardAbility : Ability
{
    Card _card;
    public CardAbility(Card card)
    {
        _card = card;
    }
}
public abstract class AgentAbility : Ability
{
    Agent _agent;
    public AgentAbility(Agent agent)
    {
        _agent = agent;
    }
}
