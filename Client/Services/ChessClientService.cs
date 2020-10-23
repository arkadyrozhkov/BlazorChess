using BlazorChess.Client.Model;
using BlazorChess.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;

namespace BlazorChess.Client.Services {
    public class ChessClientService {
        List<BoardCellModel> cellModels = new List<BoardCellModel>();
        public ReadOnlyCollection<BoardCellModel> BoardCellModels => cellModels.AsReadOnly();



        public BoardCellModel SelectedCellModel { get; set; }

        public Player Player { get; set; }
        public Game Game { get; set; }
        public void Init(string playerId) {
            Player = Game.Player1.Id == playerId ? Game.Player1 : Game.Player2;
            Game.CurrentTurn = Game.Player1;
            Game.Pieces.ForEach(p => p.Game = Game);
            for(int i = 0; i < 8; i++) {
                for(int j = 0; j < 8; j++) {
                    cellModels.Add(new BoardCellModel(i, j) {
                        Piece = Game.Pieces.FirstOrDefault(p => p.Position.X == i && p.Position.Y == j)
                    });
                }
            }
        }
        public void MoveTo(Position from, Position to) {
            var fromModel = cellModels.FirstOrDefault(p => p.X == from.X && p.Y == from.Y);
            var toModel = cellModels.FirstOrDefault(p => p.X == to.X && p.Y == to.Y);
            Game.CurrentTurn = Game.CurrentTurn == Game.Player1 ? Game.Player2 : Game.Player1;
            toModel.Piece = fromModel.Piece;
            fromModel.Piece = null;
        }
        public void DropState() {
            foreach(var model in BoardCellModels) {
                model.IsSelected = false;
                model.IsMovementHint = false;
            }
        }
    }
}
