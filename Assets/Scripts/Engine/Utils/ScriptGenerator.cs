using UnityEditor;
using System.IO;
using UnityEngine;

public class ScriptGenerator
{
    private static string generatedScriptsPath = "Assets/Resources/Definitions/Generated/";

    public static void GenerateDefinitions()
    {
        CreateGenerationDirectory();
        ClearGeneratedScripts();

        foreach (DynamicEnum def in Resources.LoadAll<DynamicEnum>("Definitions"))
        {
            def.UpdateValues();
        }
        AssetDatabase.Refresh();
    }

    public static string GeneratePhaseScript(string className)
    {
        
        string code = ($"public class {className} : Phase" + "{\n");
        code += @"
    public override void Enter(Phase prevPhase, GameState state){}
    public override void Exit(Phase nextPhase, GameState state){}
}";
        return code;
    }
    public static string GenerateInitializerScript(string className)
    {
        
        string code = ($"using UnityEngine;\npublic class {className} : Initializer" + "{\n");
        code += @"
    // Initialize a GameState
    public override GameState Initialize()
    {
        GameState state = new GameState();
        // Initialization logic here
        return state;
    }
}";
        return code;
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

    [MenuItem("Assets/Create/MosAIc/Initializer", false, 80)]
    public static void CreateInitializer()
    {
        string path = GetSelectedPathOrFallback();
        string fileName = "NewInitializer.cs";
        string fullPath = Path.Combine(path, fileName);

        if (File.Exists(fullPath))
        {
            EditorUtility.DisplayDialog("Error", "A file with this name already exists.", "OK");
            return;
        }

        string scriptContent = 
@"using UnityEngine;

public class NewInitializer : Initializer
{
    // Initialize a GameState
    public override GameState Initialize()
    {
        GameState state = new GameState();
        // Initialization logic here
        return state;
    }
}";

        File.WriteAllText(fullPath, scriptContent);
        AssetDatabase.Refresh();

        // Select the new file in the Project window
        UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(fullPath);
        Selection.activeObject = asset;
    }

    private static string GetSelectedPathOrFallback()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (string.IsNullOrEmpty(path))
        {
            return "Assets";
        }
        else if (Path.HasExtension(path))
        {
            return Path.GetDirectoryName(path);
        }

        return path;
    }
}
