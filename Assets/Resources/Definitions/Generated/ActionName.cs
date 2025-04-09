public enum ActionName {
    Default,
    [ClassMapping(typeof(StartGameAction))]
    StartGame,
    [ClassMapping(typeof(PlayCardAction))]
    PlayCard,
}