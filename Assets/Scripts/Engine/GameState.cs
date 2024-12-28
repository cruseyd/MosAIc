using System;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    private Dictionary<PhaseName, Phase> phases;
    private Dictionary<Pair<CardZoneName, int>, CardZone> zones;
    private Dictionary<int, Player> players;

    public GameState()
    {
        zones = new Dictionary<Pair<CardZoneName, int>, CardZone>();
        players = new Dictionary<int, Player>();
        phases = new Dictionary<PhaseName, Phase>();
        foreach (PhaseName phaseName in Enum.GetValues(typeof(PhaseName)))
        {
            phases[phaseName] = (Phase)phaseName.GetAssociatedClass();
            Debug.Log("Added phase " + phaseName);
        }
    }

    public GameState(GameState state)
    {
        zones = new Dictionary<Pair<CardZoneName, int>, CardZone>();
        players = new Dictionary<int, Player>();
        foreach (Pair<CardZoneName, int> key in state.zones.Keys)
        {
            zones[key] = new CardZone(state.zones[key]);
        }
        foreach (int key in state.players.Keys)
        {
            players[key] = new Player(state.players[key]);
        }
    }
}
