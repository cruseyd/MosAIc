using UnityEngine;
using UnityEditor;

public class ScriptGenerationWindow : EditorWindow
{

    [MenuItem("Window/MosAIc/ScriptGeneration")]
    public static void ShowWindow()
    {
        GetWindow<ScriptGenerationWindow>("ScriptGeneration");
    }
    
    void OnGUI()
    {
        
    }
}