using BlazorChess.Shared;
using System.Collections.Generic;
using System.Linq;

namespace BlazorChess.Server.Models {
    public class Queen : Piece {
        private bool TryAddPosition(List<Position> moves, List<Position> alliesPositions, List<Position> enemiesPositions, int x, int y) {
            if(alliesPositions.Any(p => p.X == x && p.Y == y)) {
                return false;
            }
            moves.Add(new Position(x, y));
            if(enemiesPositions.Any(p => p.X == x && p.Y == y)) {
                return false;
            }
            return true;
        }
        public override IList<Position> GetAllowableMoves() {
            var accesableMoves = new List<Position>();
            var alliesPositions = Game.Pieces.Where(p => p.Color == Color).Select(p => p.Position).ToList();
            var enemiesPositions = Game.Pieces.Where(p => p.Color != Color).Select(p => p.Position).ToList();
            for(int x = Position.X + 1, y = Position.Y + 1; x < 8 && y < 8; x++, y++) {
                if(!TryAddPosition(accesableMoves, alliesPositions, enemiesPositions, x, y)) {
                    break;
                }
            }
            for(int x = Position.X + 1, y = Position.Y - 1; x < 8 && y >= 0; x++, y--) {
                if(!TryAddPosition(accesableMoves, alliesPositions, enemiesPositions, x, y)) {
                    break;
                }
            }
            for(int x = Position.X - 1, y = Position.Y + 1; x >= 0 && y < 8; x--, y++) {
                if(!TryAddPosition(accesableMoves, alliesPositions, enemiesPositions, x, y)) {
                    break;
                }
            }
            for(int x = Position.X - 1, y = Position.Y - 1; x >= 0 && y >= 0; x--, y--) {
                if(!TryAddPosition(accesableMoves, alliesPositions, enemiesPositions, x, y)) {
                    break;
                }
            }
            for(int i = Position.X + 1; i < 8; i++) {
                if(!TryAddPosition(accesableMoves, alliesPositions, enemiesPositions, i, Position.Y)) {
                    break;
                }
            }
            for(int i = Position.X - 1; i >= 0; i--) {
                if(!TryAddPosition(accesableMoves, alliesPositions, enemiesPositions, i, Position.Y)) {
                    break;
                }
            }
            for(int i = Position.Y + 1; i < 8; i++) {
                if(!TryAddPosition(accesableMoves, alliesPositions, enemiesPositions, Position.X, i)) {
                    break;
                }
            }
            for(int i = Position.Y - 1; i >= 0; i--) {
                if(!TryAddPosition(accesableMoves, alliesPositions, enemiesPositions, Position.X, i)) {
                    break;
                }
            }
            return accesableMoves.AsReadOnly();
        }
    }
}
