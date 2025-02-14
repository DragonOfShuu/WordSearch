namespace WordSearch.Server.Models.GameLogic
{
    public class GameBoard
    {
        public Difficulty Difficulty { get; set; } = Difficulty.Empty;
        public string[][] BoardCharacters { get; set; } = [];
        public WordDictionary Findable { get; set; } = [];
        public string[] Found { get; set; } = [];
        public long Started { get; set; } // milliseconds

        public static Board toBoard(GameBoard gameBoard)
        {
            throw new NotImplementedException();
        }
    }

    public class Board
    {
        public Difficulty Difficulty { get; set; } = Difficulty.Empty;
        public string[][] BoardCharacters { get; set; } = [];
        public string[] Findable { get; set; } = [];
        public WordDictionary Found { get; set; } = [];
        public long Started { get; set; } // milliseconds
    }
}
