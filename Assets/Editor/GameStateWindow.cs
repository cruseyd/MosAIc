using UnityEngine;
using UnityEditor;

public class GameStateWindow : EditorWindow
{
    private GameState gameState;
    private Vector2 scrollPosition = Vector2.zero;
    [MenuItem("Window/MosAIc/GameState")]
    public static void ShowWindow()
    {
        GetWindow<GameStateWindow>("GameState");
    }
    private void PrintState()
    {
        GUILayout.Label("Game State", EditorStyles.boldLabel);
        if (Application.isPlaying)
        {
            gameState = GameManager.state;
        }
        if (gameState == null)
        {
            EditorGUILayout.LabelField("GameState is null");
        } else {
            GUILayout.Label("Current Phase: " + gameState.phase.ToString());
            //GUILayout.Label("Active Agent ID: " + gameState.player + " | type: " + gameState.activeAgent.type.ToString());
            GUILayout.Label("======================================================================");
            GUILayout.Label("Agents:",EditorStyles.boldLabel);
            for ( int id = 0; id < gameState.NumAgents(); id++)
            {
                GUILayout.Label(gameState.GetAgent(id).ToString());
            }
            GUILayout.Label("======================================================================");
            GUILayout.Label("Card Zones:",EditorStyles.boldLabel);
            foreach (CardZone zone in gameState.GetCardZones())
            {
                GUILayout.Label("----------------------------------------------------------------------");
                GUILayout.Label(zone.ToString());
                foreach (CardIndex card in zone.Cards())
                {
                    GUILayout.Label("\t" + gameState.GetCard(card).ToString());
                }
            }
        }
    }
    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, true);
        PrintState();
        EditorGUILayout.EndScrollView();
    }
}
