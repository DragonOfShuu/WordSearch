namespace WordSearch.Server.Models.GameLogic
{
    public class FindWordResults
    {
        public GameBoard GameBoard { get; set; } = new GameBoard();
        public string[] WordsFound { get; set; } = [];
    }

    public class FindWordResultsForClient
    {
        public Board Board { get; set; } = new Board();
        public string[] WordsFound { get; set; } = [];
        public int XpGain { get; set; }
    }
}
