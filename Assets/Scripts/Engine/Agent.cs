using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Agent
{
    public int ID {get; private set;}
    public AgentType type {get; private set;}
    private Dictionary<StatName, Stat> stats = new Dictionary<StatName, Stat>();
    public Agent(AgentType type_, int id_)
    {
        type = type_;
        ID = id_;
        stats.Clear();
        foreach (StatName name in GameParams.AgentStats(type))
        {
            stats[name] = new Stat(name, GameParams.MinValue(name));
        }
    }

    public Agent(Agent agent)
    {
        type = agent.type;
        ID = agent.ID;
        stats = agent.stats;
    }

    public void SetStat(StatName name, int value)
    {
        Debug.Assert(stats.ContainsKey(name));
        stats[name].value = value;
    }
    public void IncrementStat(StatName name, int delta)
    {
        Debug.Assert(stats.ContainsKey(name));
        stats[name].value = stats[name].value + delta;
    }

    public int GetStat(StatName name)
    {
        Debug.Assert(stats.ContainsKey(name));
        return stats[name].value;
    }

    public override string ToString()
    {
        string info =  "Agent | type: " + type.ToString() + " | ID: " + ID;
        foreach (Stat stat in stats.Values)
        {
            info += " | " + stat.name.ToString() + " = " + stat.value;
        }
        return info;
    }

}
