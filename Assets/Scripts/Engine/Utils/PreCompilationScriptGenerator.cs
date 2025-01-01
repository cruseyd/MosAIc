using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using System.IO;

[InitializeOnLoad]
public static class PreCompilationScriptGenerator
{
    private static string generatedScriptsPath = "Assets/Resources/Definitions/Generated";
    static PreCompilationScriptGenerator()
    {
        // Subscribe to the assemblyCompilationStarted event
        CompilationPipeline.compilationStarted += OnAssemblyCompilationStarted;
    }

    private static void OnAssemblyCompilationStarted(object obj)
    {
        GenerateScripts();
    }

    
    private static void GenerateScripts()
    {
        CreateGenerationDirectory();
        ClearGeneratedScripts();

        foreach (DynamicEnum def in Resources.LoadAll<DynamicEnum>("Definitions"))
        {
            def.UpdateValues();
        }
        AssetDatabase.Refresh();
    }
    private static void CreateGenerationDirectory()
    {
        if (!Directory.Exists(generatedScriptsPath))
        {
            Directory.CreateDirectory(generatedScriptsPath);
        }
    }
    private static void ClearGeneratedScripts()
    {
        if (Directory.Exists(generatedScriptsPath))
        {
            foreach (string file in Directory.GetFiles(generatedScriptsPath, "*.cs"))
            {
                File.Delete(file);
            }
        }
    }
}
