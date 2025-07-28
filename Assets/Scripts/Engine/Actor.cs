using System.Collections.Generic;
using UnityEngine;

public abstract class Actor
{
    public int playerID { get; private set; }
    public Actor(int _player = 0) { playerID = _player; }

    public virtual List<GameAction> GetValidActions(GameState state, GameRules rules) { return new List<GameAction>(); }
    public virtual GameAction ChooseAction(List<GameAction> validActions) { return null; }
}