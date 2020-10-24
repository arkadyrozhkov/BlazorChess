using BlazorChess.Server.Models;
using BlazorChess.Shared;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorChess.Server.Services {
    public class ChessHub : Hub {
        private const string FindGameGroupName = "FindGame";
        private readonly ChessService chessService;
        private Player CurrentPlayer => chessService.Players.Find(p => p.Id == Context.ConnectionId);
        public ChessHub(ChessService chessService) {
            this.chessService = chessService;
        }
        public async override Task OnConnectedAsync() {
            chessService.Players.Add(new Player(Context.ConnectionId));
            await Groups.AddToGroupAsync(Context.ConnectionId, FindGameGroupName);
            await base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception) {
            CurrentPlayer.Game.SetWinner(CurrentPlayer.Color == PieceColor.White ? PieceColor.Black : PieceColor.White);
            chessService.Players.Remove(CurrentPlayer);
            return base.OnDisconnectedAsync(exception);
        }
        public async Task MoveTo(Position from, Position to) {
            var game = CurrentPlayer.Game;
            var attackingPiece = game.Pieces.FirstOrDefault(p => p.Position == from);
            if(attackingPiece != null && attackingPiece.MoveTo(to)) {
                game.CurrentTurn = CurrentPlayer == game.Player1 ? game.Player1 : game.Player2;
                await Clients.Client(game.Player1.Id).SendAsync("PieceMoved", from, to);
                await Clients.Client(game.Player2.Id).SendAsync("PieceMoved", from, to);
            }
        }
        public Position[] GetAllowableMoves(Position position) {
            var piece = CurrentPlayer.Game.Pieces.FirstOrDefault(p => p.Position == position);
            if(piece != null) {
                return piece.GetAllowableMoves().ToArray();
            }
            return Array.Empty<Position>();
        }
        public async Task FindGame() {
            CurrentPlayer.Status = PlayerStatus.FindingGame;
            await Clients.Client(CurrentPlayer.Id).SendAsync("AddedToQueue");
            if(chessService.Players.Count(p => p.Status == PlayerStatus.FindingGame) > 1) {
                var players = chessService.Players.Where(p => p.Status == PlayerStatus.FindingGame).Take(2).ToList();
                var game = chessService.StartGame(players[0], players[1]);
                game.GameFinished += (winner, looser) => {
                    // await Clients.Client(winner.Id).SendAsync("IsWinner");
                    // await Clients.Client(looser.Id).SendAsync("IsLooser");
                };
                foreach(var player in players) {
                    player.Status = PlayerStatus.PlayingGame;
                    Shared.Game clientGame = new Shared.Game {
                        Pieces = game.Pieces.Select(p => new Shared.Piece {
                            Color = p.Color,
                            Position = p.Position,
                            Type = Enum.Parse<PieceType>(p.GetType().Name)
                        }).ToList(),
                        PlayerColor = player.Color,
                        PlayerId = player.Id,
                        Status = player.Color == PieceColor.White ? GameStatus.MyTurn : GameStatus.EnemyTurn
                    };
                    var _game = JsonConvert.SerializeObject(clientGame);
                    await Clients.Client(player.Id).SendAsync("StartGame", _game);
                }
            }
        }
    }
}
