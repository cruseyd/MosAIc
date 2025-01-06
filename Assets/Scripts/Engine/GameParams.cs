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
    [SerializeField] private GameMode gameMode;
    [SerializeField] private List<StatRange> statRanges;
    [SerializeField] private List<ParamValue> paramValues;

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
}
