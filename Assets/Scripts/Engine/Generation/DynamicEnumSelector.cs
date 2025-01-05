using UnityEngine;

public class DynamicEnumSelector : MonoBehaviour
{
    [HideInInspector] public DynamicEnum dynamicEnum;
    [HideInInspector] public int selectedValueIndex;

    public string SelectedValue => dynamicEnum != null && dynamicEnum.values.Count > selectedValueIndex
        ? dynamicEnum.values[selectedValueIndex]
        : null;

    public int getEnumIndex() { return selectedValueIndex; }
    public string getEnumValue() { return dynamicEnum.values[selectedValueIndex]; }
    public string getEnumName() { return dynamicEnum.name; }
}