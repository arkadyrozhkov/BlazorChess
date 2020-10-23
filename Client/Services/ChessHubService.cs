using BlazorChess.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BlazorChess.Client.Services {
    public class ChessHubService {
        private HubConnection hubConnection;
        private readonly NavigationManager navigationManager;
        private readonly ChessClientService chessClientService;
        private bool IsConnected => hubConnection.State == HubConnectionState.Connected;
        public ChessHubService(NavigationManager navigationManager, ChessClientService chessClientService) {
            this.navigationManager = navigationManager;
            this.chessClientService = chessClientService;
            hubConnection = new HubConnectionBuilder()
               .WithUrl(navigationManager.ToAbsoluteUri("/chesshub"))
               .Build();
            hubConnection.On<string, string>("StartGame", (_game, playerId) => {
                chessClientService.Game = JsonConvert.DeserializeObject<Game>(_game, new JsonSerializerSettings {
                    TypeNameHandling = TypeNameHandling.Objects
                });
                chessClientService.Init(playerId);
                navigationManager.NavigateTo($"/chess");
            });
            hubConnection.On<Position, Position>("PieceMoved", (from, to) => chessClientService.MoveTo(from, to));
        }
        public async Task Init() {
            if(!IsConnected) {
                await hubConnection.StartAsync();
            }
        }
        public async Task MoveTo(Position from, Position to) => await hubConnection.SendAsync(nameof(MoveTo), from, to);
        public async Task<Position[]> GetAllowableMoves(Position position) => await hubConnection.InvokeAsync<Position[]>(nameof(GetAllowableMoves), position);
        public async Task FindGame() => await hubConnection.SendAsync(nameof(FindGame));
    }
}
