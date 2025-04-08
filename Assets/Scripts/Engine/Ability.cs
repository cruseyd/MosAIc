using System.Collections.Generic;
using UnityEngine;

public struct AbilityArgs
{
    public List<Agent> agents;
    public List<Card> cards;
    public List<StatValuePair> stats;
}
public abstract class Ability
{
    public abstract void Execute(AbilityArgs args);
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
