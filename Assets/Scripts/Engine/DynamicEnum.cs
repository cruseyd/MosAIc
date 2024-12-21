using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DynamicEnum")]
public class DynamicEnum : ScriptableObject
{
    public new string name;
    public List<string> values = new List<string>();
}