using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DynamicEnumSelector), true)]
public class DynamicEnumSelectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DynamicEnumSelector script = (DynamicEnumSelector)target;
        
        if (script.dynamicEnum != null)
        {
            script.selectedValueIndex = EditorGUILayout.Popup(
                "Selected Value",
                script.selectedValueIndex,
                script.dynamicEnum.values.ToArray()
            );
        }
        else
        {
            EditorGUILayout.HelpBox("Assign a DynamicEnum asset.", MessageType.Warning);
        }

        DrawDefaultInspector();
    }
}