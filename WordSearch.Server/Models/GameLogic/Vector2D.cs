
namespace WordSearch.Server.Models.GameLogic
{
    public class Vector2D
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2D() { }

        public Vector2D Change(Func<int, int> change)
        {
            var newX = change(X);
            var newY = change(Y);
            return new Vector2D(newX, newY);
        }

        public Vector2D Change(Func<int, int> changeX, Func<int, int> changeY)
        {
            var newX = changeX(X);
            var newY = changeY(Y);
            return new Vector2D(newX, newY);
        }

        public override bool Equals(object? obj)
        {
            return obj is Vector2D d &&
                   X == d.X &&
                   Y == d.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"[Vector2D={X}:{Y}]";
        }
    }
}
