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
        public Dictionary<string, WordType> Findable { get; set; } = [];
        public string[] Found { get; set; } = [];
        public long Started { get; set; } // milliseconds

        public Dictionary<string, WordType> FoundAsWordType()
        {
            return Found.Select(foundWord => Findable[foundWord]).ToDictionary(dict => dict.Word, dict => dict);
        }

        public Board ToBoard()
        {
            var newFound = Found.Select(foundWord => Findable[foundWord]).ToDictionary(dict => dict.Word, dict => dict);

            return new Board()
            {
                Difficulty = Difficulty,
                BoardCharacters = BoardCharacters,
                Findable = [.. Findable.Keys],
                Found = newFound,
                Started = Started
            };
        }

        public bool AllWordsFound()
        {
            return Found.Length == Findable.Count;
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
        public Dictionary<string, WordType> Found { get; set; } = [];
        public long Started { get; set; } // milliseconds
    }

    public class BoardUpdate
    {
        public GameBoard Board { get; set; } = new GameBoard();
        public GameBoard? NewBoard { get; set; }
        public string[] WordsFound { get; set; } = [];
        public int XpGain { get; set; } = 0;

        public BoardUpdateClient ForClient()
        {
            return new BoardUpdateClient()
            {
                Board = Board.ToBoard(),
                NewBoard = NewBoard == null ? null : NewBoard.ToBoard(),
                WordsFound = WordsFound,
                XpGain = XpGain
            };
        }
    }

    public class BoardUpdateClient
    {
        public Board Board { get; set; } = new Board();
        public Board? NewBoard { get; set; }
        public string[] WordsFound { get; set; } = [];
        public int XpGain { get; set; } = 0;
    }
}
