using System.IO;
using UnityEditor;
using UnityEngine;

public abstract class GameAssetData : ScriptableObject
{
    public virtual void OnValidate()
    {
        Debug.Log("GameAssetData::OnValidate");
        GenerateAbility();
    }
    protected void GenerateAbility()
    {
        string className = $"{name}_Ability";
        string abilityDirectory = CreateAbilityFolder();
        string abilityScriptName = $"{className}.cs";
        string abilityScriptPath = Path.Combine(abilityDirectory, abilityScriptName);
        if (!File.Exists(abilityScriptPath))
        {
            string code = GenerateAbilityScript(className);
            File.WriteAllText(abilityScriptPath, code);
            AssetDatabase.Refresh();
        }
    }
    protected abstract string GenerateAbilityScript(string className);
    protected string CreateAbilityFolder()
    {
        string subdirectoryName = "Abilities";
        string assetPath = AssetDatabase.GetAssetPath(this);
        string assetDirectory = Path.GetDirectoryName(assetPath);
        string subdirectoryPath = Path.Combine(assetDirectory, subdirectoryName);
        if (!Directory.Exists(subdirectoryPath))
        {
            AssetDatabase.CreateFolder(assetDirectory, subdirectoryName);
        }
        return subdirectoryPath;
    }
}
