public enum PhaseName{
    [ClassMapping(typeof(MainPhase))]
    Main,
    [ClassMapping(typeof(DrawPhase))]
    Draw,
    [ClassMapping(typeof(EndPhase))]
    End,
}