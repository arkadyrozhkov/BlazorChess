using Newtonsoft.Json;
using System;

namespace BlazorChess.Shared {
    public enum PlayerStatus { MainMenu, FindingGame, PlayingGame }
    public class Player {
        public Player(string id) {
            Id = id;
        }
        public string Id { get; private set; }
        public PieceColor Color { get; set; } = PieceColor.Undefined;
        public PlayerStatus Status { get; set; } = PlayerStatus.MainMenu;
        [JsonIgnore]
        public Game Game { get; set; }
    }
}
