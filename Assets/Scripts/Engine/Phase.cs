
public abstract class Phase
{
    
    public PhaseName name;
    public virtual void Enter(Phase prevPhase){}
    public virtual void Exit(Phase nextPhase){}
}
