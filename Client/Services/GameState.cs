using BlazorChess.Client.Model;
using BlazorChess.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BlazorChess.Client.Services {
    public class GameState {
        private PlayerStatus status = PlayerStatus.MainMenu;
        public List<BoardCellModel> BoardCellModels { get; } = new List<BoardCellModel>();
        public Game Game { get; set; }
        public BoardCellModel SelectedCellModel { get; set; }
        public PlayerStatus PlayerStatus {
            get => status;
            set {
                status = value;
                PlayerStatusChanged?.Invoke();
            }
        }
        public event Action PlayerStatusChanged;
    }
}
