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
    // What happens at the start of this phase
    public override void Enter(Phase prevPhase, GameState state){}
    
    // What happens when this phase ends
    public override void Exit(Phase nextPhase, GameState state){}
    
    // What happens when the player manually ends this phase
    public override Phase Next(GameState state)
    {
        return base.Next(state);
    }
}";
        return code;
    }
    public static string GenerateGameRulesScript(string className)
    {
        
        string code = ($"using UnityEngine;\npublic class {className} : GameRules" + "{\n");
        code += @"
    // Initialize a GameState
    public override GameState Initialize()
    {
        GameState state = new GameState();
        // Initialization logic here
        return state;
    }
    public override bool IsValid(ActionName action)
    {
        switch(action) {
            default: return true;
        }
    }
}";
        return code;
    }
    public static string GenerateGameActionScript(string className)
    {
        
        string code = ($"public class {className} : GameAction" + " {\n");
        code += ($"    public {className}(int agentID, GameActionArgs args, GameState state) : base(agentID, args, state)" + "{}");
        code += @"
    protected override void Execute(GameState state)
    {
        // Get game objects from state
        // Add GameEffect instances using this.AddEffect
    }
};";
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

    [MenuItem("Assets/Create/MosAIc/GameRules", false, 80)]
    public static void CreateGameRules()
    {
        string path = GetSelectedPathOrFallback();
        string fileName = "NewGameRules.cs";
        string fullPath = Path.Combine(path, fileName);

        if (File.Exists(fullPath))
        {
            EditorUtility.DisplayDialog("Error", "A file with this name already exists.", "OK");
            return;
        }

        string scriptContent = 
@"using UnityEngine;

public class NewGameRules : GameRules
{
    // Initialize a GameState
    public override GameState Initialize()
    {
        GameState state = new GameState();
        // Initialization logic here
        return state;
    }
    public override bool IsValid(ActionName action)
    {
        switch(action) {
            default: return true;
        }
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
