using BlazorChess.Shared;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorChess.Server.Services {
    public class ChessHub : Hub {
        private readonly ChessService chessService;
        public ChessHub(ChessService chessService) {
            this.chessService = chessService;
        }
        const string FindGameGroupName = "FindGame";
        private Player CurrentPlayer => chessService.Players.Find(p => p.Id == Context.ConnectionId);
        public async override Task OnConnectedAsync() {
            chessService.Players.Add(new Player(Context.ConnectionId));
            await Groups.AddToGroupAsync(Context.ConnectionId, FindGameGroupName);
            await base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception) {
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
            if(chessService.Players.Count(p => p.Status == PlayerStatus.FindingGame) > 1) {
                var players = chessService.Players.Where(p => p.Status == PlayerStatus.FindingGame).Take(2).ToList();
                var game = chessService.StartGame(players[0], players[1]);
                foreach(var player in players) {
                    player.Status = PlayerStatus.PlayingGame;
                    var _game = JsonConvert.SerializeObject(game, Formatting.Indented, new JsonSerializerSettings {
                        TypeNameHandling = TypeNameHandling.Objects,
                        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
                    });
                    await Clients.Client(player.Id).SendAsync("StartGame", _game, player.Id);
                }
            }
        }
    }
}
