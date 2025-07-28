using System.Collections.Generic;


public abstract class Ability
{
    public abstract List<GameEffect> Execute(GameActionArgs args);
    public List<CardFilter> targets { get; protected set; }
}
public abstract class CardAbility : Ability
{
    Card _card;
    public CardAbility(Card card)
    {
        targets = new List<CardFilter>();
        _card = card;
    }
}
public abstract class AgentAbility : Ability
{
    Agent _agent;
    public AgentAbility(Agent agent)
    {
        targets = new List<CardFilter>();
        _agent = agent;
    }
}
