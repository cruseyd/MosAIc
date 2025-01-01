using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatValuePair {
    [SerializeField] public StatName stat;
    [SerializeField] public int value;
}

[CreateAssetMenu(fileName = "NewCardData", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardData : ScriptableObject {
    public new string name;
    public List<StatValuePair> stats;

}