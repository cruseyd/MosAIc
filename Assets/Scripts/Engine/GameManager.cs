using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : Singleton<GameManager>
{
    public static event Action<GameActionWithEffects, GameState, GameState> onTakeAction;
    public static event Action<Targeter> onStartTargeting;
    public static event Action<Targeter, CardIndex> onAddTarget;
    public static event Action<Targeter> onEndTargeting;
    public static GameState state { get; private set; }
    public static GameRules rules { get; private set; }
    public static GameActionArgs currentActionArgs = null;
    public static Targeter targeter = null;
    public static Previewer previewer = null;
    public static CardIndex previewing
    {
        get
        {
            CardZone previewZone = state.GetCardZone(CardZoneName.Preview);
            return previewZone.NumCards() > 0 ? previewZone.Cards()[0] : null;
        }
    }
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscape();
        }
    }
    public void TakeAction(ActionName actionName, int player, GameActionArgs args = null)
    {
        if (Targeting()) { return; }
        if (!rules.IsValid(actionName, player, args, state))
        {
            Debug.Log($"Invalid Action: {actionName}");
            return;
        }
        GameAction action = actionName.GetAssociatedClass(player, args, state) as GameAction;
        var actionWithEffects = action.Resolve();
        GameState oldState = state;
        state = actionWithEffects.state;
        onTakeAction?.Invoke(actionWithEffects, oldState, state);
    }
    public static bool Targeting() { return targeter != null; }
    public void StartTargeting(Card source)
    {
        targeter = new Targeter(source.id, source.GetTargets());
        onStartTargeting?.Invoke(targeter);
    }

    public void AddTarget(Card target)
    {
        Debug.Assert(Targeting());
        bool success = targeter.add(target);
        if (success)
        {
            onAddTarget?.Invoke(targeter, target.id);
        }
        if (targeter.finished())
            {
                Debug.Log("Targeter is finished. Playing the card");
                var args = new GameActionArgs();
                args.cards.Add(targeter.source);
                foreach (CardIndex ti in targeter.getTargets()) { args.targets.Add(ti); }
                EndTargeting();
                instance.TakeAction(ActionName.PlayCard, state.currentPlayer, args);
            }
    }

    public void EndTargeting()
    {
        if (!Targeting()) { return; }
        onEndTargeting?.Invoke(targeter);
        targeter = null;
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
    public virtual void HandleEscape() {}

}
