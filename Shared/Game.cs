using System;
using System.Collections.Generic;

namespace BlazorChess.Shared {
    public class Game {
        private GameStatus status;
        public List<Piece> Pieces { get; set; }
        public string PlayerId { get; set; }
        public PieceColor PlayerColor { get; set; }
        public GameStatus Status {
            get => status;
            set {
                status = value;
                StatusChanged?.Invoke();
            }
        }

        public event Action StatusChanged;
    }
}