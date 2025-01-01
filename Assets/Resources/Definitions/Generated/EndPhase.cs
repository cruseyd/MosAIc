public class EndPhase : Phase{
    public EndPhase(PhaseName name_) : base(name_){}

            public override void Enter(Phase prevPhase){}
            public override void Exit(Phase nextPhase){}
        }