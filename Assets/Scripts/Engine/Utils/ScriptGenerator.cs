using UnityEditor;
using System.IO;

public class CustomScriptGenerator
{
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
