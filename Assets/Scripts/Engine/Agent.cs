using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Agent
{
    public int ID {get; private set;}
    public AgentType type {get; private set; }
    public AgentData data {get; private set; }
    private Dictionary<StatName, int> stats = new Dictionary<StatName, int>();
    public Agent(AgentType type_, int id_)
    {
        type = type_;
        ID = id_;
        stats.Clear();
        foreach (StatName name in GameParams.AgentStats(type))
        {
            stats[name] = GameParams.MinValue(name);
        }
    }

    public Agent(AgentData data_, int id_)
    {
        data = data_;
        type = data_.type;
        ID = id_;
        stats.Clear();
        foreach (var statPair in data.baseStats)
        {
            stats[statPair.stat] = statPair.value;
        }
    }
    public Agent(Agent agent)
    {
        data = agent.data;
        type = agent.type;
        ID = agent.ID;
        stats = agent.stats;
    }

    public void SetStat(StatName name, int value)
    {
        Debug.Assert(stats.ContainsKey(name), $"Stat {name} missing from Agent");
        stats[name] = value;
    }
    public void IncrementStat(StatName name, int delta)
    {
        Debug.Assert(stats.ContainsKey(name), $"Stat {name} missing from Agent");
        stats[name]= stats[name] + delta;
    }

    public int GetStat(StatName name)
    {
        Debug.Assert(stats.ContainsKey(name), $"Stat {name} missing from Agent");
        return stats[name];
    }

    public override string ToString()
    {
        string info =  "Agent | type: " + type.ToString() + " | ID: " + ID;
        foreach (var item in stats)
        {
            info += " | " + item.Key + " = " + item.Value;
        }
        return info;
    }

}
