using BlazorChess.Shared;
using System.Collections.Generic;
using System.Linq;

namespace BlazorChess.Server.Models {
    public abstract class Piece {
        public PieceColor Color { get; set; }
        public Position Position { get; set; }
        public Game Game { get; set; }
        public abstract IList<Position> GetAllowableMoves();
        public virtual bool MoveTo(Position position) {
            if(GetAllowableMoves().Contains(position)) {
                var attackedPiece = Game.Pieces.FirstOrDefault(p => p.Position == position);
                if(attackedPiece != null) {
                    Game.Pieces.Remove(attackedPiece);
                    if(attackedPiece is King king) {
                        Game.SetWinner(Color);
                    }
                }
                Position = position;
                return true;
            }
            return false;
        }
    }
}
