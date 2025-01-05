
using System;

public abstract class Phase
{
    public PhaseName name 
    {
        get {
            string typeString = this.GetType().ToString().Replace("Phase","");
            return (PhaseName)Enum.Parse(typeof(PhaseName), typeString);
        }
    }
    public virtual void Enter(Phase prevPhase, GameState state){}
    public virtual void Exit(Phase nextPhase, GameState state){}
    public virtual Phase Next(GameState state)
    {
        PhaseName nextPhase = name.GetNext();
        return (Phase)nextPhase.GetAssociatedClass();
    }
}
