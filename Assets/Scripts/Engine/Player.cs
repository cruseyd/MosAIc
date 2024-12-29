using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Agent
{
    public int ID {get; private set;}
    public AgentType type {get; private set;}


    public Agent(AgentType type_, int id_)
    {
        type = type_;
        ID = id_;
    }

    public Agent(Agent agent)
    {
        type = agent.type;
        ID = agent.ID;
    }

}
