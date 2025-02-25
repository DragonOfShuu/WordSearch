namespace WordSearch.Server.Models.GameLogic
{
    public class WordType
    {
        public string Word { get; set; } = string.Empty;
        public Vector2D Position { get; set; } = new Vector2D();
        public Vector2D Rotation { get; set; } = new Vector2D();
    }
}
