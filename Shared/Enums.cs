namespace BlazorChess.Shared {
    public enum GameStatus { MyTurn, EnemyTurn, Victory, Loss }
    public enum PieceColor { White, Black }
    public enum PieceType { Pawn, Knight, Rook, Bishop, Queen, King }
    public enum PlayerStatus { MainMenu, FindingGame, PlayingGame }
}
