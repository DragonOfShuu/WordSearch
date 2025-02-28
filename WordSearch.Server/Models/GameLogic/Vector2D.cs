namespace WordSearch.Server.Models.GameLogic
{
    public class Vector2D
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return $"[Vector2D={X}:{Y}]";
        }
    }
}
