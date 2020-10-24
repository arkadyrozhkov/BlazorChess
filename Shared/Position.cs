namespace BlazorChess.Shared {
    public struct Position {
        public static Position Invalid = new Position(-1, -1);
        public Position(int x, int y) {
            X = x;
            Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj) => obj is Position position && X == position.X && Y == position.Y;

        public static bool operator ==(Position p1, Position p2) => p1.Equals(p2);
        public static bool operator !=(Position p1, Position p2) => !p1.Equals(p2);
        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();
    }
}
