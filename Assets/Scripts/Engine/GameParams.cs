using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct CardStatInput {
    public string name;
    public int minValue;
    public int maxValue;
}

class GameParams : MonoBehaviour 
{
    [SerializeField] public List<CardStatInput> cardStats;
}