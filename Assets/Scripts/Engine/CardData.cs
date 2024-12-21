using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardData : ScriptableObject {
    public new string name;
    public SerializableDictionary<string, int> stats;
}