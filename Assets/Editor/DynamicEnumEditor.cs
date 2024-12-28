using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DynamicEnum), true)]
public class DynamicEnumEditor : Editor
{
     public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DynamicEnum dynamicEnum = (DynamicEnum)target;

        if (GUILayout.Button("Generate"))
        {
            dynamicEnum.UpdateValues();
            EditorUtility.SetDirty(dynamicEnum);
        }
    }
}
