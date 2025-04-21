public enum PhaseName {
    Default,
    [ClassMapping(typeof(GameStartPhase))]
    GameStart,
    [ClassMapping(typeof(PlayerMainPhase))]
    PlayerMain,
}