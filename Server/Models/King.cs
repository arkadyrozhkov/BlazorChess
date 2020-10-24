using BlazorChess.Shared;
using System.Collections.Generic;
using System.Linq;

namespace BlazorChess.Server.Models {
    public class King : Piece {
        private void TryAddPosition(List<Position> moves, List<Position> alliesPositions, int x, int y) {
            if(x >= 0 && x <= 7 && y >= 0 && y <= 7 && !alliesPositions.Any(p => p.X == x && p.Y == y)) {
                moves.Add(new Position(x, y));
            }
        }
        public override IList<Position> GetAllowableMoves() {
            var accesableMoves = new List<Position>();
            var alliesPositions = Game.Pieces.Where(p => p.Color == Color).Select(p => p.Position).ToList();
            TryAddPosition(accesableMoves, alliesPositions, Position.X, Position.Y + 1);
            TryAddPosition(accesableMoves, alliesPositions, Position.X, Position.Y - 1);
            TryAddPosition(accesableMoves, alliesPositions, Position.X + 1, Position.Y);
            TryAddPosition(accesableMoves, alliesPositions, Position.X - 1, Position.Y);
            TryAddPosition(accesableMoves, alliesPositions, Position.X + 1, Position.Y + 1);
            TryAddPosition(accesableMoves, alliesPositions, Position.X - 1, Position.Y - 1);
            TryAddPosition(accesableMoves, alliesPositions, Position.X - 1, Position.Y + 1);
            TryAddPosition(accesableMoves, alliesPositions, Position.X + 1, Position.Y - 1);
            return accesableMoves.AsReadOnly();
        }
    }
}
