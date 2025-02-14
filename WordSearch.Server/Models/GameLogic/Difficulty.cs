namespace WordSearch.Server.Models.GameLogic
{
    public class Difficulty
    {
        public int Level { get; set; }
        public string Intensity { get; set; } = string.Empty; // medium
        public Vector2D Size { get; set; } = new Vector2D();
        public int Time { get; set; } // in seconds

        public static Difficulty Empty { get {  return new Difficulty(); } }
    }
}
