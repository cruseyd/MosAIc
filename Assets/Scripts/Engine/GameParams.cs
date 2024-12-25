using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct StatRange {
    [SerializeField] public StatName stat;
    [SerializeField] public int minValue;
    [SerializeField] public int maxValue;
}

class GameParams : MonoBehaviour 
{
    [SerializeField] public List<StatRange> cardStats;
    [SerializeField] public List<StatRange> playerStats;
}