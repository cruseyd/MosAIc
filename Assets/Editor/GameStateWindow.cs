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
            GUILayout.Label("Current Phase: " + gameState.phase.ToString());
            GUILayout.Label("Active Agent ID: " + gameState.activeAgent.ID + " | type: " + gameState.activeAgent.type.ToString());
            GUILayout.Label("======================================================================");
            GUILayout.Label("Agents:",EditorStyles.boldLabel);
            for ( int id = 0; id < gameState.NumAgents(); id++)
            {
                GUILayout.Label(gameState.GetAgentWithID(id).ToString());
            }
            GUILayout.Label("======================================================================");
            GUILayout.Label("Card Zones:",EditorStyles.boldLabel);
            foreach (CardZone zone in gameState.GetCardZones())
            {
                GUILayout.Label("----------------------------------------------------------------------");
                GUILayout.Label(zone.ToString());
                foreach (Card card in zone.Cards())
                {
                    GUILayout.Label(card.ToString());
                }
            }
        }
    }
}
