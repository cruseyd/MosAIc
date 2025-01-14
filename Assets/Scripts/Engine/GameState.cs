using System;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public event Action<GameEffect, GameState> onResolveEffect;
    public int activeAgentID;
    public Agent activeAgent
    {
        get {
            Debug.Assert(agents.ContainsKey(activeAgentID));
            return agents[activeAgentID];
        }
    }
    public PhaseName phase {get; set;}
    private Dictionary<Pair<CardZoneName, int>, CardZone> zones
        = new Dictionary<Pair<CardZoneName, int>, CardZone>();
    private Dictionary<int, Agent> agents = new Dictionary<int, Agent>();
    private Queue<GameEffect> effectQueue = new Queue<GameEffect>();
    public GameState()
    {
        activeAgentID = -1;
        phase = PhaseName.Default;

    }
    public GameState(GameState state)
    {
        zones = new Dictionary<Pair<CardZoneName, int>, CardZone>();
        agents = new Dictionary<int, Agent>();

        foreach (Pair<CardZoneName, int> key in state.zones.Keys)
        {
            zones[key] = state.zones[key].Clone();
        }
        foreach (int key in state.agents.Keys)
        {
            agents[key] = new Agent(state.agents[key]);
        }
        phase = state.phase;
        activeAgentID = state.activeAgentID;
    }

    public void Execute(GameEffect effect)
    {
        GameEffect currentEffect = effect;
        onResolveEffect?.Invoke(currentEffect, this);
        currentEffect.Execute();
        while (currentEffect.simultaneous != null)
        {
            currentEffect = currentEffect.simultaneous;
            onResolveEffect?.Invoke(currentEffect, this);
            currentEffect.Execute();
        }
        while(effectQueue.Count > 0)
        {
            Execute(effectQueue.Dequeue());
        }
    }
    public void AddAgent(AgentType type, int id)
    {
        if (agents.ContainsKey(id))
        {
            Debug.LogError($"GameState.AddAgent | Error: Tried to add agent with existing id {id}");
            return;
        }
        agents[id] = new Agent(type, id);
    }

    public Agent GetAgentWithID(int id)
    {
        if (!agents.ContainsKey(id))
        {
            Debug.LogError($"GameState.GetAgent | Error: Could not find Agent with id {id}");
        }
        return agents[id];
    }

    public int NumAgents() { return agents.Count; }
    public void AddCardZone<T> (CardZoneName name, int agent) where T : CardZone, new()
    {
        var key = new Pair<CardZoneName, int>();
        key.first = name;
        key.second = agent;
        if (zones.ContainsKey(key))
        {
            Debug.LogError($"GameState.AddCardZone | Error: Key already exists ({name},{agent})");
        }
        
        T zone = CardZone.Create<T>(name, agent);
        zones[key] = zone;
    }
    public void AddDeck(CardZoneName name, int agent)
    {
        var key = new Pair<CardZoneName, int>();
        key.first = name;
        key.second = agent;
        if (zones.ContainsKey(key))
        {
            Debug.LogError($"GameState.AddCardZone | Error: Key already exists ({name},{agent})");
        }
        var zone = new Deck(name, agent);
        zones[key] = zone;
    }
    public List<CardZone> GetCardZones()
    {
        var cardZones = new List<CardZone>();
        foreach (CardZone zone in zones.Values)
        {
            cardZones.Add(zone);
        }
        return cardZones;
    }
    public CardZone GetCardZone(CardZoneName name, int agent)
    {
        var key = new Pair<CardZoneName, int>();
        key.first = name;
        key.second = agent;
        if (!zones.ContainsKey(key))
        {
            Debug.LogError($"GameState.GetCardZone | Error: Key missing ({name},{agent})");
        }
        return zones[key];
    }
    public Deck GetDeck(CardZoneName name, int agent)
    {
        var key = new Pair<CardZoneName, int>();
        key.first = name;
        key.second = agent;
        if (!zones.ContainsKey(key))
        {
            Debug.LogError($"GameState.GetCardZone | Error: Key missing ({name},{agent})");
        }
        Debug.Assert(zones[key].GetType() == typeof(Deck));
        return (Deck)zones[key];
    }
}
