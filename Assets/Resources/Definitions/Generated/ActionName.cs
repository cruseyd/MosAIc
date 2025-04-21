public enum ActionName {
    Default,
    [ClassMapping(typeof(StartGameAction))]
    StartGame,
    [ClassMapping(typeof(PlayCardAction))]
    PlayCard,
    [ClassMapping(typeof(BuyCardAction))]
    BuyCard,
    [ClassMapping(typeof(ChangePhaseAction))]
    ChangePhase,
}