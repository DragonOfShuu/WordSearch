namespace WordSearch.Server.Models.GameLogic
{
    /// <summary>
    /// The internal GameBoard for the server. This
    /// will include *ALL* of the necessary information
    /// for the board to work.
    /// </summary>
    public class GameBoard
    {
        public Difficulty Difficulty { get; set; } = Difficulty.Empty;
        public string[][] BoardCharacters { get; set; } = [];
        public WordDictionary Findable { get; set; } = [];
        public string[] Found { get; set; } = [];
        public long Started { get; set; } // milliseconds

        public Board ToBoard()
        {
            throw new NotImplementedException();
        }
    }

    public class Board
    {
        /// <summary>
        /// The public Board for the client. This is
        /// exactly like the GameBoard, BUT the findable
        /// list does not include coordinates.
        /// </summary>
        public Difficulty Difficulty { get; set; } = Difficulty.Empty;
        public string[][] BoardCharacters { get; set; } = [];
        public string[] Findable { get; set; } = [];
        public WordDictionary Found { get; set; } = [];
        public long Started { get; set; } // milliseconds
    }
}
