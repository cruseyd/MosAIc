using System;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public int activeAgentID;
    public PhaseName phase {get; set;}
    private Dictionary<PhaseName, Phase> phases;
    private Dictionary<Pair<CardZoneName, int>, CardZone> zones;
    private Dictionary<int, Agent> agents;

    public GameState()
    {
        Debug.Log("GameState constructor");
        zones = new Dictionary<Pair<CardZoneName, int>, CardZone>();
        agents = new Dictionary<int, Agent>();
        activeAgentID = -1;
        phase = PhaseName.Default;
    }

    public GameState(GameState state)
    {
        zones = new Dictionary<Pair<CardZoneName, int>, CardZone>();
        agents = new Dictionary<int, Agent>();

        foreach (Pair<CardZoneName, int> key in state.zones.Keys)
        {
            zones[key] = new CardZone(state.zones[key]);
        }
        foreach (int key in state.agents.Keys)
        {
            agents[key] = new Agent(state.agents[key]);
        }
        phase = state.phase;
        activeAgentID = state.activeAgentID;
    }

    public void AddAgent(Agent agent)
    {
        int id = agent.ID;
        if (agents.ContainsKey(id))
        {
            Debug.LogError($"GameState.AddAgent | Error: Tried to add agent with existing id {id}");
            return;
        }
        agents[id] = agent;
    }

    public Agent GetAgentWithID(int id)
    {
        if (!agents.ContainsKey(id))
        {
            Debug.LogError($"GameState.GetAgent | Error: Could not find Agent with id {id}");
        }
        return agents[id];
    }

    public void AddCardZone(CardZoneName name, int agent)
    {
        var key = new Pair<CardZoneName, int>();
        key.first = name;
        key.second = agent;
        if (zones.ContainsKey(key))
        {
            Debug.LogError($"GameState.AddCardZone | Error: Key already exists ({name},{agent})");
        }
        var zone = new CardZone(name, agent);
        zones[key] = zone;
    }
}
