using System.Collections.Generic;
using UnityEngine;
public class Targeter
{
    public CardIndex source { get; private set; }
    private List<CardFilter> filters = new List<CardFilter>();
    private List<CardIndex> targets = new List<CardIndex>();

    public Targeter(CardIndex card, List<CardFilter> filters)
    {
        source = card;
        this.filters = filters;
    }

    public List<CardIndex> getTargets()
    {
        return targets;
    }

    public bool finished()
    {
        return targets.Count == filters.Count;
    }

    public CardFilter next()
    {
        Debug.Assert(!finished());
        return filters[targets.Count];
    }

    public bool add(Card card)
    {
        if (next().Compare(card))
        {
            targets.Add(card.id);
            return true;
        }
        return false;
    }

}