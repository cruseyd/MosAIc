using System;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public static GameState main = null;
    public Agent activeAgent {get; set; }
    public PhaseName currentPhase
    {
        get {
            if (currentPhase_ == null) { return PhaseName.Default; }
            return currentPhase_.name;
        } 
        set {
            if (!phases.ContainsKey(value))
            {
                Debug.LogError($"Invalid PhaseName: {value.ToString()}");
            }
            Phase nextPhase = phases[value];
            Phase prevPhase = currentPhase_;
            if (prevPhase != null && (nextPhase.name != prevPhase.name))
            {
                prevPhase?.Exit(nextPhase, this);
            }
            currentPhase_ = nextPhase;
            nextPhase.Enter(prevPhase, this);
        }
    }
    private Phase currentPhase_;
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
            if (phaseName == PhaseName.Default) { continue; }
            phases[phaseName] = (Phase)phaseName.GetAssociatedClass();
            Debug.Log("Added phase " + phaseName);
        }
        activeAgent = null;
        currentPhase_ = null;
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
        currentPhase = state.currentPhase;
        activeAgent = agents[state.activeAgent.ID];
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
}
