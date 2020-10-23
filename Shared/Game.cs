using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BlazorChess.Shared {
    public class Game {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public List<Piece> Pieces { get; } = new List<Piece>();
        public string Id { get; } = Guid.NewGuid().ToString();
        [JsonIgnore]
        public Player CurrentTurn { get; set; }
        private void Initialize() {
            Player1.Color = PieceColor.White;
            Player2.Color = PieceColor.Black;
            CurrentTurn = Player1;
            CreateMajorPiecesLine(PieceColor.Black, 0);
            CreatePawnsList(PieceColor.Black, 1);
            CreatePawnsList(PieceColor.White, 6);
            CreateMajorPiecesLine(PieceColor.White, 7);
        }
        private void CreatePawnsList(PieceColor color, int rowIndex) {
            for(int i = 0; i < 8; i++) {
                Pieces.Add(new Pawn() { Color = color, Position = new Position(rowIndex, i), Game = this });
            }

        }
        private void CreateMajorPiecesLine(PieceColor color, int rowIndex) {
            Pieces.Add(new Rook() { Color = color, Position = new Position(rowIndex, 0), Game = this });
            Pieces.Add(new Knight() { Color = color, Position = new Position(rowIndex, 1), Game = this });
            Pieces.Add(new Bishop() { Color = color, Position = new Position(rowIndex, 2), Game = this });
            Pieces.Add(new Queen() { Color = color, Position = new Position(rowIndex, 3), Game = this });
            Pieces.Add(new King() { Color = color, Position = new Position(rowIndex, 4), Game = this });
            Pieces.Add(new Bishop() { Color = color, Position = new Position(rowIndex, 5), Game = this });
            Pieces.Add(new Knight() { Color = color, Position = new Position(rowIndex, 6), Game = this });
            Pieces.Add(new Rook() { Color = color, Position = new Position(rowIndex, 7), Game = this });
        }
        public Game() {

        }
        public Game(Player player1, Player player2) {
            Player1 = player1;
            Player2 = player2;
            Player1.Game = this;
            Player2.Game = this;
            Initialize();
        }
    }
}
