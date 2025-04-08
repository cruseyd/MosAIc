using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct StatValuePair {
    [SerializeField] public StatName stat;
    [SerializeField] public int value;
}

[CreateAssetMenu(fileName = "NewCardData", menuName = "Scriptable Objects/CardData", order = 1)]
public class CardData : GameAssetData {
    public GameObject prefab = null;
    public new string name;
    [TextArea(3,10)]
    public List<string> text = new List<string>();
    public CardType type;
    public List<StatValuePair> baseStats;

    public override void OnValidate()
    {
        base.OnValidate();
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
    public int GetBaseStatValue(StatName statName)
    {
        foreach (var statValue in baseStats)
        {
            if (statValue.stat == statName)
            {
                return statValue.value;
            }
        }
        Debug.LogError($"CardData.GetBaseStatValue | Error: Could not find StatName: {statName.ToString()}");
        return 0;
    }
    public string GetText(int index)
    {
        Debug.Assert(text.Count > index);
        return text[index];
    }
    public CardAbility GetAbility(Card card)
    {
        Debug.Assert(card != null);
        string abilityName = AbilityClassName();
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type abilityType = assembly.GetType(abilityName);
            if (abilityType != null && typeof(Ability).IsAssignableFrom(abilityType))
            {
                return (CardAbility)Activator.CreateInstance(abilityType, card);
            }
        }
        Debug.LogError("CardData::GetAbility | Could not find ability with name " + abilityName);
        return null;
    }
    protected override string GenerateAbilityScript()
    {
        string className = AbilityClassName();
        string code = $@"
public class {className} : CardAbility {{

    public {className}(Card card) : base(card) {{}}

    public override void Execute(AbilityArgs args) {{}}
}};
";
        return code;
    }
}