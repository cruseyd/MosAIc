
public abstract class Phase
{
    public virtual void Enter(Phase prevPhase){}
    public virtual void Exit(Phase nextPhase){}
}
