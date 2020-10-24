namespace BlazorChess.Shared {
    public class Piece {
        public PieceColor Color { get; set; }
        public PieceType Type { get; set; }
        public Position Position { get; set; }
    }
}