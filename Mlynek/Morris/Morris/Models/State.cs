namespace Morris.Models
{
    public enum PlayersTurn
    {
        Player1,
        Player2
    }

    public enum GameState
    {
        InGame,
        Off,
        PlacingStones,
        RemovingStone
    }

    public enum FieldState
    {
        Empty = 0,
        P1 = 1,
        P2 = 2
    }
}