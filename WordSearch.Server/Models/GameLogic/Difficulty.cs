namespace WordSearch.Server.Models.GameLogic
{
    public class Difficulty
    {
        /// <summary>
        /// The level of difficulty of the board
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// The intensity curve that the level
        /// increase will take
        /// </summary>
        public string Intensity { get; set; } = string.Empty; // medium
        /// <summary>
        /// Optional static size
        /// </summary>
        public Vector2D? Size { get; set; } = new Vector2D();
        /// <summary>
        /// How long the player has to complete the board
        /// </summary>
        public int Time { get; set; } // in seconds

        public static Difficulty Empty { get {  return new Difficulty(); } }
    }
}
