using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAgentData", menuName = "Scriptable Objects/AgentData", order = 2)]
public class AgentData : ScriptableObject {
    public GameObject prefab = null;
    public new string name;
    public AgentType type;
    public List<StatValuePair> baseStats;

    private void OnValidate()
    {
        UpdateDefaultStats();
    }
    public void GenerateDefaultStats()
    {
        baseStats.Clear();
        foreach (StatName stat in GameParams.AgentStats(type))
        {
            var pair = new StatValuePair();
            pair.stat = stat;
            pair.value = GameParams.MinValue(stat);
            baseStats.Add(pair);
        }
    }
    public void UpdateDefaultStats()
    {
        if (!ValidStats())
        {
            GenerateDefaultStats();
        }

    }
    private bool ValidStats()
    {
        var correctStats = GameParams.AgentStats(type);
        var currentStats = new List<StatName>();
        foreach (var stat in baseStats)
        {
            currentStats.Add(stat.stat);
        }
        correctStats.Sort();
        currentStats.Sort();
        if (correctStats.Count != currentStats.Count) { return false; }
        for( int ii = 0; ii < currentStats.Count; ii++)
        {
            if (correctStats[ii] != currentStats[ii]) { return false; }
        }
        return true; 
    }
    public int GetBaseStatValue(StatName statName)
    {
        foreach (var statValue in baseStats)
        {
            if (statValue.stat == statName)
            {
                return statValue.value;
            }
        }
        Debug.LogError($"AgentData.GetBaseStatValue | Error: Could not find StatName: {statName.ToString()}");
        return 0;
    }
}