using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Agent
{
    public int ID {get; private set;}
    public AgentType type {get; private set;}
    private List<Stat> stats;
    public Agent(AgentType type_, int id_)
    {
        type = type_;
        ID = id_;
        stats = new List<Stat>();
    }

    public Agent(Agent agent)
    {
        type = agent.type;
        ID = agent.ID;
        stats = agent.stats;
    }

    public override string ToString()
    {
        string info =  "Agent | type: " + type.ToString() + " | ID: " + ID;
        foreach (Stat stat in stats)
        {
            info += " | " + stat.name.ToString() + " = " + stat.value;
        }
        return info;
    }

}
