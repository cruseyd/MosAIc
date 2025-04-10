using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : Singleton<GameManager>
{
    public static GameState state {get; private set; }
    public static event Action<Card> onCardDoubleClick;
    public Phase phase
    {
        get {
            return _phase;
        } 
        private set {
            Phase nextPhase = value;
            Phase prevPhase = _phase;
            if (prevPhase != null && (nextPhase.name != prevPhase.name))
            {
                prevPhase?.Exit(nextPhase, state);
            }
            _phase = nextPhase;
            nextPhase.Enter(prevPhase, state);
        }
    }
    private Phase _phase;
    protected override void Awake()
    {
        base.Awake();
    }
    protected void Start()
    {
        GameMode mode = GameParams.GameMode();
        Initializer initializer = (Initializer)mode.GetAssociatedClass();
        state = initializer.Initialize();
    }

    void Update()
    {
        // Listen for user input
    }
    public void TakeAction(ActionName actionName, int agentID, GameActionArgs args = null)
    {
        GameAction action = actionName.GetAssociatedClass(agentID, args, state) as GameAction;
        var actionWithEffects = action.Resolve();
        GameStateUI.Animate(actionWithEffects);
        state = actionWithEffects.state;
        
    }

    public void TryAction(ActionName action)
    {
        // Used by the AI to produce new game states to analyze
    }

    public static void HandleDoubleClick(IDoubleClickable clickedObject, PointerEventData eventData)
    {
        if (clickedObject is CardUI)
        {
            Card card = ((CardUI)clickedObject).card;
            onCardDoubleClick?.Invoke(card);
        }
    }

    public static void HandleRightClick(IRightClickable clickedObject, PointerEventData eventData)
    {

    }
}
