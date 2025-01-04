public enum PhaseName {
    Default,
    [ClassMapping(typeof(ReadyPhase))]
    Ready,
    [ClassMapping(typeof(SetPhase))]
    Set,
    [ClassMapping(typeof(DrawPhase))]
    Draw,
    [ClassMapping(typeof(MainPhase))]
    Main,
    [ClassMapping(typeof(EndPhase))]
    End,
    [ClassMapping(typeof(GameStartPhase))]
    GameStart,
    [ClassMapping(typeof(GameEndPhase))]
    GameEnd,
}