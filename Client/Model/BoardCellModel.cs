using BlazorChess.Shared;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BlazorChess.Client.Model {
    public class BoardCellModel : INotifyPropertyChanged {
        private Piece piece;
        private bool movementHint;
        private bool isSelected;
        public BoardCellModel(int x, int y) {
            X = x;
            Y = y;
        }
        public int X { get; private set; }
        public int Y { get; private set; }
        public Piece Piece { get => piece; set { piece = value; OnPropertyChanged(); } }
        public bool IsMovementHint { get => movementHint; set { movementHint = value; OnPropertyChanged(); } }
        public bool IsSelected { get => isSelected; set { isSelected = value; OnPropertyChanged(); } }
        private void OnPropertyChanged([CallerMemberName] string memberName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
