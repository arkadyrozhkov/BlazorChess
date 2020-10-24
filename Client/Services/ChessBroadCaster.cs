using BlazorChess.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BlazorChess.Client.Services {
    public interface IChessBroadCaster {
        Task MoveToAsync(Position from, Position to);
        Task<Position[]> GetAllowableMovesAsync(Position position);
        Task FindGameAsync();
        Task ConnectionStartAsync();
        Action<Game> StartGame { get; set; }
        Action AddedToQueue { get; set; }
        Action IsWinner { get; set; }
        Action IsLooser { get; set; }
        Action<Position, Position> PieceMoved { get; set; }
    }
    public class ChessBroadCaster : IChessBroadCaster {
        private readonly HubConnection hubConnection;
        private bool IsConnected => hubConnection.State == HubConnectionState.Connected;
        public ChessBroadCaster(NavigationManager navigationManager) {
            hubConnection = new HubConnectionBuilder()
               .WithUrl(navigationManager.ToAbsoluteUri("/chesshub"))
               .Build();
            hubConnection.On(nameof(StartGame), (string _game) => {
                var game = JsonConvert.DeserializeObject<Game>(_game);
                StartGame?.Invoke(game);
            });
            hubConnection.On(nameof(AddedToQueue), () => AddedToQueue?.Invoke());
            hubConnection.On(nameof(IsWinner), () => IsWinner?.Invoke());
            hubConnection.On(nameof(IsLooser), () => IsLooser?.Invoke());
            hubConnection.On(nameof(PieceMoved), (Position from, Position to) => PieceMoved?.Invoke(from, to));
        }
        public Action<Game> StartGame { get; set; }
        public Action AddedToQueue { get; set; }
        public Action IsWinner { get; set; }
        public Action IsLooser { get; set; }
        public Action<Position, Position> PieceMoved { get; set; }
        public async Task ConnectionStartAsync() {
            if(!IsConnected) {
                await hubConnection.StartAsync();
            }
        }
        public async Task MoveToAsync(Position from, Position to) => await hubConnection.SendAsync("MoveTo", from, to);
        public async Task<Position[]> GetAllowableMovesAsync(Position position) => await hubConnection.InvokeAsync<Position[]>("GetAllowableMoves", position);
        public async Task FindGameAsync() => await hubConnection.SendAsync("FindGame");
    }
}
