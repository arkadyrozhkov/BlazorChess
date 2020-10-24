using BlazorChess.Client.Model;
using BlazorChess.Shared;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorChess.Client.Services {
    public class ChessClientService {
        private readonly GameState gameState;
        private readonly IChessBroadCaster chessBroadCaster;
        private void DropState() {
            foreach(var model in gameState.BoardCellModels) {
                model.IsSelected = false;
                model.IsMovementHint = false;
            }
        }
        private void StartGame(Game game) {
            gameState.Game = game;
            for(int i = 0; i < 8; i++) {
                for(int j = 0; j < 8; j++) {
                    gameState.BoardCellModels.Add(new BoardCellModel(i, j) {
                        Piece = gameState.Game.Pieces.FirstOrDefault(p => p.Position.X == i && p.Position.Y == j)
                    });
                }
            }
            gameState.PlayerStatus = PlayerStatus.PlayingGame;
        }
        private void AddedToQueue() => gameState.PlayerStatus = PlayerStatus.FindingGame;
        private void IsWinner() => gameState.Game.Status = GameStatus.Victory;
        private void IsLooser() => gameState.Game.Status = GameStatus.Loss;
        private void PieceMoved(Position from, Position to) {
            var fromModel = gameState.BoardCellModels.FirstOrDefault(p => p.X == from.X && p.Y == from.Y);
            var toModel = gameState.BoardCellModels.FirstOrDefault(p => p.X == to.X && p.Y == to.Y);
            gameState.Game.Status = gameState.Game.Status == GameStatus.MyTurn ? GameStatus.EnemyTurn : GameStatus.MyTurn;
            toModel.Piece = fromModel.Piece;
            fromModel.Piece = null;
        }
        public async Task CellClickedAsync(BoardCellModel model) {
            if(gameState.Game.Status == GameStatus.EnemyTurn) return;
            if(gameState.SelectedCellModel != null) {
                await chessBroadCaster.MoveToAsync(new Position(gameState.SelectedCellModel.X, gameState.SelectedCellModel.Y), new Position(model.X, model.Y));
                gameState.SelectedCellModel = null;
                DropState();
            }
            else {
                if(model.Piece != null && model.Piece.Color == gameState.Game.PlayerColor) {
                    gameState.SelectedCellModel = model;
                    model.IsSelected = true;
                    foreach(var piece in await chessBroadCaster.GetAllowableMovesAsync(new Position(model.X, model.Y))) {
                        gameState.BoardCellModels.First(m => m.X == piece.X && m.Y == piece.Y).IsMovementHint = true;
                    }
                }
            }
        }
        public async Task FindGameAsync() {
            await chessBroadCaster.ConnectionStartAsync();
            await chessBroadCaster.FindGameAsync();
        }
        public ChessClientService(GameState gameState, IChessBroadCaster chessBroadCaster) {
            this.gameState = gameState;
            this.chessBroadCaster = chessBroadCaster;
            chessBroadCaster.StartGame = StartGame;
            chessBroadCaster.AddedToQueue = AddedToQueue;
            chessBroadCaster.IsWinner = IsWinner;
            chessBroadCaster.IsLooser = IsLooser;
            chessBroadCaster.PieceMoved = PieceMoved;
        }
    }
}
