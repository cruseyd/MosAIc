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
    public List<StatValuePair> stats;

    private void OnEnable()
    {
        if (stats == null)
        {
            stats = new List<StatValuePair>();
        }
        if (stats.Count == 0)
        {
            foreach (StatName stat in Enum.GetValues(typeof(StatName)))
            {
                var pair = new StatValuePair();
                pair.stat = stat;
                pair.value = GameParams.MinValue(stat);
                stats.Add(pair);
            }
        }
    }

}