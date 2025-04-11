using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState
{
    public static event Action<GameEffect, GameState> onResolveEffect;
    public PhaseName phase {get; set;}
    private Dictionary<CardZoneID, CardZone> zones
        = new Dictionary<CardZoneID, CardZone>();
    private Dictionary<int, Agent> agents
        = new Dictionary<int, Agent>();
    private Dictionary<CardIndex, Card> cards
        = new Dictionary<CardIndex, Card>();
    private Queue<GameEffect> effectQueue = new Queue<GameEffect>();
    // Construction
    public GameState()
    {
        phase = PhaseName.Default;
    }
    public GameState(GameState state)
    {
        foreach (var (player, agent) in state.agents)
        {
            agents[player] = (Agent)Activator.CreateInstance(agent.GetType(), agent);
        }
        foreach (var (key, zone) in state.zones)
        {
            zones[key] = (CardZone)Activator.CreateInstance(zone.GetType(), zone);
        }
        foreach (var (index, card) in state.cards)
        {
            cards[index] = (Card)Activator.CreateInstance(card.GetType(), card);
        }
        phase = state.phase;
    }
    // Add Assets
    public void AddAgent(AgentType type, int player)
    {
        if (agents.ContainsKey(player))
        {
            Debug.LogError($"GameState.AddAgent | Error: Tried to add agent with existing id {player}");
            return;
        }
        agents[player] = new Agent(type, player);
    }
    public void AddAgent(Agent agent)
    {
        int player = agent.player;
        Debug.Assert(!agents.ContainsKey(player), $"GameState already contains an Agent with key {player}");
        agents[player] = agent;
    }
    public void AddCardZone<T> (CardZoneID id) where T : CardZone, new()
    {
        Debug.Assert(!zones.ContainsKey(id),$"GameState.AddCardZone | Error: Key already exists ({id.name},{id.player})");
        
        T zone = CardZone.Create<T>(id);
        zones[id] = zone;
    }
    public void AddCardZonesFromUI()
    {
        zones.Clear();
        List<CardZoneUI> uiZones = GameStateUI.GetAllCardZoneUI();
        foreach (CardZoneUI z in uiZones)
        {
            switch (z)
            {
                case DeckUI:
                    AddCardZone<Deck>(z.id);
                    break;
                default:
                    AddCardZone<LinearCardZone>(z.id);
                    break;
            }
        }
    }
    public void AddCard(Card card, CardZoneID zoneID)
    {
        Debug.Assert(!cards.ContainsKey(card.id));
        CardZone zone = GetCardZone(zoneID);
        Debug.Assert(!zone.Contains(card.id));
        MoveCard(card.id, zoneID);
    }
    // Accessors
    public Agent GetAgent(int player)
    {
        if (!agents.ContainsKey(player))
        {
            Debug.LogError($"GameState.GetAgent | Error: Could not find Agent with id {player}");
        }
        return agents[player];
    }
    public CardZone GetCardZone(CardZoneID id)
    {
        foreach (var (key, value) in zones)
        {
            Debug.Log($"{key.name}, {key.player} -> {value.id.name}");
        }
        Debug.Assert(zones.ContainsKey(id),$"GameState.GetCardZone | Error: Key missing ({id.name},{id.player})"); 
        return zones[id];
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
    public Card GetCard(CardIndex index)
    {
        Debug.Assert(cards.ContainsKey(index));
        return cards[index];
    }
    public int NumAgents() { return agents.Count; }

    // Execute Effect
    public List<GameEffect> Execute(GameEffect effect)
    {
        var list = new List<GameEffect>();
        GameEffect currentEffect = effect;
        onResolveEffect?.Invoke(currentEffect, this);
        list.Add(currentEffect);
        currentEffect.Execute(this);
        while (currentEffect.simultaneous != null)
        {
            currentEffect = currentEffect.simultaneous;
            onResolveEffect?.Invoke(currentEffect, this);
            list.Add(currentEffect);
            currentEffect.Execute(this);
        }
        while(effectQueue.Count > 0)
        {
            list.AddRange(Execute(effectQueue.Dequeue()));
        }
        return list;
    }

    // Mutators
    public void MoveCard(CardIndex cardIndex, CardZoneID toZoneID, int position = 0)
    {
        Debug.Assert(cards.ContainsKey(cardIndex));
        Card card = GetCard(cardIndex);
        if (card.zone != null)
        {
            CardZone prevZone = GetCardZone(card.zone);
            prevZone.Remove(card.id);
        }
        CardZone toZone = GetCardZone(toZoneID);
        toZone.AddAtPosition(cardIndex, position);
        card.zone = toZoneID;
    }

    public Card DrawCard(CardZoneID deckID, CardZoneID toZoneID, int position = 0)
    {
        Deck deck = (Deck)GetCardZone(deckID);
        Card drawnCard = GetCard(deck.Draw());
        MoveCard(drawnCard.id, toZoneID, position);
        return drawnCard;
    }

    public void IncrementAgentStat(int player, StatName stat, int delta)
    {
        var agent = GetAgent(player);
        agent.IncrementStat(stat, delta);
    }
}
