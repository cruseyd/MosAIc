using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.IO.LowLevel.Unsafe;

[CreateAssetMenu(menuName = "Scriptable Objects/DynamicEnum")]
public class DynamicEnum : ScriptableObject
{
    public List<string> values = new List<string>();
    protected bool generateClassMaps = false;
    protected string classMapFileName = "NAME_VALUE";
    public virtual void UpdateValues()
    {
        if (generateClassMaps)
        {
            GenerateClassMaps();
        }
        GenerateEnumCode();
    }

    protected virtual string GenerateClassName(string name, string value)
    {
        string className = classMapFileName;
        className = className.Replace("NAME", SanitizeName(name));
        className = className.Replace("VALUE", SanitizeName(value));
        return SanitizeName(className);
    }

    protected virtual string GenerateClassCode(string classname)
    {
        string code = ($"public class {classname}" + "{}");
        return code;
    }
    protected virtual void GenerateClassMaps()
    {
        foreach (var value in values)
        {
            string classname = GenerateClassName(name, value);
            string GeneratedFilePath = $"Assets/Resources/Definitions/Generated/{classname}.cs";
            if (File.Exists(GeneratedFilePath)) { continue; }

            string code = GenerateClassCode(classname);
            
            File.WriteAllText(GeneratedFilePath, code);
            Debug.Log($"DynamicEnum generated class map at {GeneratedFilePath}");

            UnityEditor.AssetDatabase.Refresh();
        }
    }

    protected virtual void GenerateEnumCode()
    {
        string GeneratedFilePath = $"Assets/Resources/Definitions/Generated/{name}.cs";
        string enumCode = ($"public enum {name}" + "{\n");
        foreach (var value in values)
        {
            if (generateClassMaps)
            {
                string classname = GenerateClassName(name, value);
                enumCode += $"    [ClassMapping(typeof({classname}))]\n";
            }
            if (!string.IsNullOrWhiteSpace(value))
            {
                enumCode += $"    {SanitizeName(value)},\n";
            }
        }
        enumCode += "}";

        File.WriteAllText(GeneratedFilePath, enumCode);
        Debug.Log($"DynamicEnum generated at {GeneratedFilePath}");
        UnityEditor.AssetDatabase.Refresh();
    }

    private string SanitizeName(string name)
    {
        return name.Replace(" ", "_").Replace("-", "_").Replace(".", "_");
    }
}