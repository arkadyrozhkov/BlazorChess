using BlazorChess.Server.Models;
using System.Collections.Generic;

namespace BlazorChess.Server.Services {
    public class ChessService {
        private List<Game> Games = new List<Game>();
        public List<Player> Players = new List<Player>();
        public Game StartGame(Player player1, Player player2) {
            var game = new Game(player1, player2);
            Games.Add(game);
            return game;
        }
    }
}
