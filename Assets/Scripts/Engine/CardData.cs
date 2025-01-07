using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatValuePair {
    [SerializeField] public StatName stat;
    [SerializeField] public int value;
}

[CreateAssetMenu(fileName = "NewCardData", menuName = "Scriptable Objects/CardData", order = 1)]
public class CardData : ScriptableObject {
    public new string name;
    public CardType type;
    public List<StatValuePair> baseStats;

    private void OnValidate()
    {
        UpdateDefaultStats();
    }
    public void GenerateDefaultStats(CardType cardType)
    {
        baseStats.Clear();
        foreach (StatName stat in GameParams.CardStats(cardType))
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
            GenerateDefaultStats(type);
        }

    }
    private bool ValidStats()
    {
        var correctStats = GameParams.CardStats(type);
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
}