public enum PhaseName{
    [ClassMapping(typeof(MainPhase))]
    Main,
    [ClassMapping(typeof(DrawPhase))]
    Draw,
    [ClassMapping(typeof(ActionPhase))]
    Action,
    [ClassMapping(typeof(EndPhase))]
    End,
}