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
public class ParamValue
{
    [SerializeField] public Parameter paramName;
    [SerializeField] public int paramValue;
}

[CreateAssetMenu(fileName = "GameParams", menuName = "Scriptable Objects/GameParams")]
public class GameParams : ScriptableObject
{
    private static GameParams _instance;
    [SerializeField] public List<StatRange> statRanges;
    [SerializeField] public List<ParamValue> paramValues;

    public static GameParams Instance()
    {
        if (_instance == null)
        {
            _instance = Resources.Load<GameParams>("Definitions");
        }
        return _instance;
    }
    public int Get(Parameter param)
    {
        foreach (ParamValue paramValue in paramValues)
        {
            if (paramValue.paramName == param)
            {
                return paramValue.paramValue;
            }
        }
        Debug.LogError($"Missing definition for parameter: {param.ToString()}");
        return 0;
    }
    public int MinValue(StatName stat)
    {
        foreach (StatRange range in statRanges)
        {
            if (range.stat == stat)
            {
                return range.minValue;
            }
        }
        return int.MinValue;
    } 
    public int MaxValue(StatName stat)
    {
        foreach (StatRange range in statRanges)
        {
            if (range.stat == stat)
            {
                return range.maxValue;
            }
        }
        return int.MaxValue;
    }
}
