using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : Singleton<GameManager>
{
    public static GameState state {get; private set; }
    public static GameRules rules {get; private set;}
    protected override void Awake()
    {
        base.Awake();
    }
    protected void Start()
    {
        GameMode mode = GameParams.GameMode();
        rules = (GameRules)mode.GetAssociatedClass();
        state = rules.Initialize();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleSpace();
        }
        
    }
    public void TakeAction(ActionName actionName, int player, GameActionArgs args = null)
    {
        if (!rules.IsValid(actionName, player, args, state))
        {
            Debug.Log($"Invalid Action: {actionName}");
            return;
        }
        GameAction action = actionName.GetAssociatedClass(player, args, state) as GameAction;
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
            instance.DoubleClickCard(card);
        }
    }

    public virtual void DoubleClickCard(Card card)
    {
        var args = new GameActionArgs();
        args.cards.Add(card.id);
        instance.TakeAction(ActionName.PlayCard, state.currentPlayer, args);
    }

    public static void HandleRightClick(IRightClickable clickedObject, PointerEventData eventData)
    {

    }

    public virtual void HandleSpace()
    {
    }
}
