using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DynamicEnum), true)]
public class DynamicEnumEditor : Editor
{
     public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DynamicEnum dynamicEnum = (DynamicEnum)target;

        if (GUILayout.Button("Update Values"))
        {
            dynamicEnum.UpdateValues();
            EditorUtility.SetDirty(dynamicEnum);
        }
    }
}
