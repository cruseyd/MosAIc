using UnityEngine;
using UnityEditor;

public class GameStateWindow : EditorWindow
{
    private GameState gameState;

    [MenuItem("Window/MosAIc/GameState")]
    public static void ShowWindow()
    {
        GetWindow<GameStateWindow>("GameState");
    }
    
    void OnGUI()
    {
        GUILayout.Label("Game State", EditorStyles.boldLabel);
        if (Application.isPlaying)
        {
            gameState = GameState.main;
        }
        if (gameState == null)
        {
            EditorGUILayout.LabelField("GameState is null");
        } else {
            GUILayout.Label("Current Phase: " + gameState.currentPhase.name.ToString());
            GUILayout.Label("Active Agent ID: " + gameState.activeAgent.ID + " | type: " + gameState.activeAgent.type.ToString());
        }
        
    }
}
