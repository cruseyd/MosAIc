using System;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    private Dictionary<PhaseName, Phase> phases;
    private Dictionary<Pair<CardZoneName, int>, CardZone> zones;
    private Dictionary<int, Agent> agents;
    public Agent activeAgent {get; set; }
    public Phase currentPhase {get; set; }


    public GameState()
    {
        zones = new Dictionary<Pair<CardZoneName, int>, CardZone>();
        agents = new Dictionary<int, Agent>();
        phases = new Dictionary<PhaseName, Phase>();
        foreach (PhaseName phaseName in Enum.GetValues(typeof(PhaseName)))
        {
            phases[phaseName] = (Phase)phaseName.GetAssociatedClass();
            Debug.Log("Added phase " + phaseName);
        }
        activeAgent = null;
        currentPhase = null;
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
    }
}
