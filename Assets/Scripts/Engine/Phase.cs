
public class Phase
{
    public virtual PhaseName name {get; protected set;}
    public Phase(PhaseName name_) { name = name_; }
    public virtual void Enter(Phase prevPhase){}
    public virtual void Exit(Phase nextPhase){}
}
