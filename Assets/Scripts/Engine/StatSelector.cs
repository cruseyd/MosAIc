using UnityEngine;

[ExecuteInEditMode]
public class StatSelector : DynamicEnumSelector
{
    // public string stat => this.dynamicEnum != null && this.dynamicEnum.values.Count > this.selectedValueIndex
    // ? this.dynamicEnum.values[this.selectedValueIndex]
    // : null;
    void Awake()
    {
        this.dynamicEnum = (DynamicEnum)Resources.Load<DynamicEnum>("Definitions/Stats");
    }
}
