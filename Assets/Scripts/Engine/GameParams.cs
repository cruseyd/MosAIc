using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

[System.Serializable]
public class StatRange
{
    [SerializeField] public StatName stat;
    [SerializeField] public int minValue;
    [SerializeField] public int maxValue;
}

[System.Serializable]
public class CardStatAssignment
{
    [SerializeField] public CardType cardType;
    [SerializeField] public List<StatName> stats;
}

[System.Serializable]
public class AgentStatAssignment
{
    [SerializeField] public AgentType agentType;
    [SerializeField] public List<StatName> stats;
}

[System.Serializable]
public class ParamValue
{
    [SerializeField] public Parameter paramName;
    [SerializeField] public int paramValue;
}

[CreateAssetMenu(fileName = "GameParams", menuName = "Scriptable Objects/GameParams")]
public class GameParams : ScriptableObject
{
    private static GameParams _instance;
    [SerializeField] private GameMode gameMode;
    [SerializeField] private List<StatRange> statRanges;
    [SerializeField] private List<ParamValue> paramValues;

    [SerializeField] private List<CardStatAssignment> cardStatAssignments;
    [SerializeField] private List<AgentStatAssignment> agentStatAssignments;

    public static GameMode GameMode() { return Instance().gameMode; }
    public static GameParams Instance()
    {
        if (_instance == null)
        {
            _instance = Resources.Load<GameParams>("Definitions/GameParams");
        }
        return _instance;
    }
    public static int Get(Parameter param)
    {
        foreach (ParamValue paramValue in Instance().paramValues)
        {
            if (paramValue.paramName == param)
            {
                return paramValue.paramValue;
            }
        }
        Debug.LogError($"Missing definition for parameter: {param.ToString()}");
        return 0;
    }
    public static int MinValue(StatName stat)
    {
        foreach (StatRange range in Instance().statRanges)
        {
            if (range.stat == stat)
            {
                return range.minValue;
            }
        }
        return int.MinValue;
    } 
    public static int MaxValue(StatName stat)
    {
        foreach (StatRange range in Instance().statRanges)
        {
            if (range.stat == stat)
            {
                return range.maxValue;
            }
        }
        return int.MaxValue;
    }
    public static List<StatName> CardStats(CardType type)
    {
        var stats = new List<StatName>();
        foreach (var assignment in Instance().cardStatAssignments)
        {
            if (assignment.cardType == type)
            {
                stats.AddRange(assignment.stats);
            }
        }
        return stats;
    }
    public static List<StatName> AgentStats(AgentType type)
    {
        var stats = new List<StatName>();
        foreach (var assignment in Instance().agentStatAssignments)
        {
            if (assignment.agentType == type)
            {
                stats.AddRange(assignment.stats);
            }
        }
        return stats;
    }
    private void OnEnable()
    {
        CreateParameterLists();
        InitializeParameterLists();
    }

    private void CreateParameterLists()
    {
        if (statRanges == null) { statRanges = new List<StatRange>(); }
        if (paramValues == null) { paramValues = new List<ParamValue>(); }
        if (cardStatAssignments == null) { cardStatAssignments = new List<CardStatAssignment>(); }
        if (agentStatAssignments == null) { agentStatAssignments = new List<AgentStatAssignment>(); }
    }
    private void InitializeParameterLists()
    {
        if (statRanges.Count == 0)
        {
            foreach (StatName name in Enum.GetValues(typeof(StatName)))
            {
                if (name == StatName.Default) { continue; }
                var range = new StatRange();
                range.stat = name;
                range.minValue = 0;
                range.maxValue = int.MaxValue;
                statRanges.Add(range);
            }
        }
        if (paramValues.Count == 0)
        {
            foreach (Parameter param in Enum.GetValues(typeof(Parameter)))
            {
                if (param == Parameter.Default) { continue; }
                var paramValue = new ParamValue();
                paramValue.paramName = param;
                paramValue.paramValue = 1;
                paramValues.Add(paramValue);
            }
        }
        if (cardStatAssignments.Count == 0)
        {
            foreach (CardType type in Enum.GetValues(typeof(CardType)))
            {
                if (type == CardType.Default) { continue; }
                var assignment = new CardStatAssignment();
                assignment.cardType = type;
                cardStatAssignments.Add(assignment);
            }
        }
                if (cardStatAssignments.Count == 0)
        {
            foreach (AgentType type in Enum.GetValues(typeof(AgentType)))
            {
                if (type == AgentType.Default) { continue; }
                var assignment = new AgentStatAssignment();
                assignment.agentType = type;
                agentStatAssignments.Add(assignment);
            }
        }
    }
}
