using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player
{
    public static Dictionary<int, Player> allPlayers;
    public int ID {get; private set;}
    public PlayerType type {get; private set;}

    public static Player Get(int id)
    {
        if (allPlayers == null || (!allPlayers.ContainsKey(id)))
        {
            Debug.LogWarning($"Player.cs | Attempted to find missing player id {id}");
            return null;
        }
        return allPlayers[id];
    }
    public Player(PlayerType type, int id)
    {
        if (allPlayers == null)
        {
            allPlayers = new Dictionary<int, Player>();
        }
        ID = id;
        allPlayers.Add(id, this);
    }

}
