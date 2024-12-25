using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player
{
    public int ID {get; private set;}
    public PlayerType type {get; private set;}


    public Player(PlayerType type_, int id_)
    {
        type = type_;
        ID = id_;
    }

    public Player(Player player)
    {
        type = player.type;
        ID = player.ID;
    }

}
