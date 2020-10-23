﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BlazorChess.Shared {
    public class Rook : Piece {
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
