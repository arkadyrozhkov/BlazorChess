using BlazorChess.Shared;

namespace BlazorChess.Server.Models {
    public class Player {
        public Player(string id) {
            Id = id;
        }
        public string Id { get; private set; }
        public PieceColor Color { get; set; }
        public PlayerStatus Status { get; set; } = PlayerStatus.MainMenu;
        public Game Game { get; set; }
    }
}
