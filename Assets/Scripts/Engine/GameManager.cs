using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameState state {get; private set; }
    public Phase phase
    {
        get {
            return _phase;
        } 
        set {
            Phase nextPhase = value;
            Phase prevPhase = _phase;
            if (prevPhase != null && (nextPhase.name != prevPhase.name))
            {
                prevPhase?.Exit(nextPhase, state);
            }
            _phase = nextPhase;
            nextPhase.Enter(prevPhase, state);
        }
    }
    private Phase _phase;
    void Awake()
    {
        GameMode mode = GameParams.GameMode();
        Initializer initializer = (Initializer)mode.GetAssociatedClass();
        state = initializer.Initialize();
    }
}
