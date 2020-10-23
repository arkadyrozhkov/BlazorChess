using System;
using System.Linq;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlazorChess.Shared {
    public enum PieceColor { Undefined, White, Black }
    public struct Position {
        public static Position Invalid = new Position(-1, -1);
        public Position(int x, int y) {
            X = x;
            Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public static bool operator ==(Position p1, Position p2) => p1.X == p2.X && p1.Y == p2.Y;
        public static bool operator !=(Position p1, Position p2) => !(p1 == p2);
    }
    public abstract class Piece {
        public PieceColor Color { get; set; }
        public Position Position { get; set; }
        [JsonIgnore]
        public Game Game { get; set; }
        public abstract IList<Position> GetAllowableMoves();
        public virtual bool MoveTo(Position position) {
            if(GetAllowableMoves().Contains(position)) {
                var attackedPiece = Game.Pieces.FirstOrDefault(p => p.Position == position);
                if(attackedPiece != null) {
                    Game.Pieces.Remove(attackedPiece);
                }
                Position = position;
                return true;
            }
            return false;
        }
    }
}
