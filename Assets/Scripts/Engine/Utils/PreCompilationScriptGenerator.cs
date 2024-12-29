using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

[InitializeOnLoad]
public static class PreCompilationScriptGenerator
{
    static PreCompilationScriptGenerator()
    {
        // Subscribe to the assemblyCompilationStarted event
        CompilationPipeline.compilationStarted += OnAssemblyCompilationStarted;
    }

    private static void OnAssemblyCompilationStarted(object obj)
    {
        GenerateDefaultScripts();
    }

    private static void GenerateDefaultScripts()
    {
        foreach (DynamicEnum def in Resources.LoadAll<DynamicEnum>("Definitions"))
        {
            def.UpdateValues();
        }
        AssetDatabase.Refresh();
    }
}
