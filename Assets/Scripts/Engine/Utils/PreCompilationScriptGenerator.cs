using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using System.IO;

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
        //ScriptGenerator.GenerateDefinitions();
    }
}
