using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : Singleton<GameManager>
{
    public static GameState state {get; private set; }
    public static GameRules rules {get; private set;}
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
        _phase = null;
    }
    protected void Start()
    {
        GameMode mode = GameParams.GameMode();
        rules = (GameRules)mode.GetAssociatedClass();
        state = rules.Initialize();
    }

    void Update()
    {
        HandleSpace();
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
            var args = new GameActionArgs();
            args.cards.Add(card.id);
            instance.TakeAction(ActionName.PlayCard, state.currentPlayer, args);
        }
    }

    public static void HandleRightClick(IRightClickable clickedObject, PointerEventData eventData)
    {

    }

    public static void HandleSpace()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (instance.phase == null)
            {
                instance.TakeAction(ActionName.StartGame, 0);
            }
        }
    }
}
