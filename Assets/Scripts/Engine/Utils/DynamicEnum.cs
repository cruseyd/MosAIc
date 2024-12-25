using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(menuName = "ScriptableObjects/DynamicEnum")]
public class DynamicEnum : ScriptableObject
{
    public List<string> values = new List<string>();

    public void UpdateValues()
    {
        GenerateEnumCode();
    }

    private void GenerateEnumCode()
    {
        // Generate the enum code based on the values array
        string GeneratedFilePath = $"Assets/Generated/{name}.cs";
        string enumCode = ($"public enum {name}" + "{\n");
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                enumCode += $"    {SanitizeName(value)},\n";
            }
        }
        enumCode += "}";

        // Write the code to a file
        File.WriteAllText(GeneratedFilePath, enumCode);
        Debug.Log($"DynamicEnum generated at {GeneratedFilePath}");

        // Refresh the AssetDatabase so Unity recognizes the new file
        UnityEditor.AssetDatabase.Refresh();
    }

    private string SanitizeName(string name)
    {
        // Sanitize the name to ensure it is a valid C# enum identifier
        return name.Replace(" ", "_").Replace("-", "_").Replace(".", "_");
    }
}