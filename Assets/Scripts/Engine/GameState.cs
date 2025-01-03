using System;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public static GameState main = null;
    public Agent activeAgent {get; set; }
    public Phase currentPhase {get; set; }
    private Dictionary<PhaseName, Phase> phases;
    private Dictionary<Pair<CardZoneName, int>, CardZone> zones;
    private Dictionary<int, Agent> agents;



    public GameState()
    {
        Debug.Log("GameState constructor");
        zones = new Dictionary<Pair<CardZoneName, int>, CardZone>();
        agents = new Dictionary<int, Agent>();
        phases = new Dictionary<PhaseName, Phase>();
        foreach (PhaseName phaseName in Enum.GetValues(typeof(PhaseName)))
        {
            phases[phaseName] = (Phase)phaseName.GetAssociatedClass();
            Debug.Log("Added phase " + phaseName);
        }
        activeAgent = null;
        if (main == null) { main = this; }
    }

    public GameState(GameState state)
    {
        zones = new Dictionary<Pair<CardZoneName, int>, CardZone>();
        agents = new Dictionary<int, Agent>();
        phases = state.phases;

        foreach (Pair<CardZoneName, int> key in state.zones.Keys)
        {
            zones[key] = new CardZone(state.zones[key]);
        }
        foreach (int key in state.agents.Keys)
        {
            agents[key] = new Agent(state.agents[key]);
        }
        currentPhase = phases[state.currentPhase.name];
        activeAgent = agents[state.activeAgent.ID];
    }

    public void AddAgent(Agent agent, int id)
    {
        if (agents.ContainsKey(id))
        {
            Debug.LogError($"GameState.AddAgent | Error: Tried to add agent with existing id {id}");
            return;
        }
        agents[id] = agent;
    }
}
