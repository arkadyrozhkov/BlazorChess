using System.Collections.Generic;
using System.Linq;

namespace BlazorChess.Shared {
    public class Pawn : Piece {
        private bool isFirstMove = true;
        public override bool MoveTo(Position position) {
            var success = base.MoveTo(position);
            if(success) {
                isFirstMove = false;
            }
            return success;
        }
        private bool TryAddPosition(List<Position> list, int x, int y, bool extraMove) {
            var takenPositions = Game.Pieces.Select(p => p.Position);
            if(x >= 0 && x <= 7 && y >= 0 && y <= 7 && (!extraMove || (extraMove && isFirstMove)) && !takenPositions.Any(p => p.X == x && p.Y == y)) {
                list.Add(new Position(x, y));
                return true;
            }
            return false;
        }
        private bool TryEnemyAttack(List<Position> list, int x, int y) {
            var takenEnemyPositions = Game.Pieces.Where(p => p.Color != Color).Select(p => p.Position);
            if(x >= 0 && x <= 7 && y >= 0 && y <= 7 && takenEnemyPositions.Any(p => p.X == x && p.Y == y)) {
                list.Add(new Position(x, y));
                return true;
            }
            return false;
        }
        public override IList<Position> GetAllowableMoves() {
            var accesableMoves = new List<Position>();
            if(Color == PieceColor.White) {
                for(int i = 1; i <= 2; i++) {
                    if(!TryAddPosition(accesableMoves, Position.X - i, Position.Y, i != 1)) break;
                }
                TryEnemyAttack(accesableMoves, Position.X - 1, Position.Y - 1);
                TryEnemyAttack(accesableMoves, Position.X - 1, Position.Y + 1);
            }
            else {
                for(int i = 1; i <= 2; i++) {
                    if(!TryAddPosition(accesableMoves, Position.X + i, Position.Y, i != 1)) break;
                }
                TryEnemyAttack(accesableMoves, Position.X + 1, Position.Y - 1);
                TryEnemyAttack(accesableMoves, Position.X + 1, Position.Y + 1);
            }
            return accesableMoves.AsReadOnly();
        }
    }
}
