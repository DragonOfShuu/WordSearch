namespace WordSearch.Server.Models.GameLogic
{
    public class FindWordResults
    {
        public GameBoard GameBoard { get; set; } = new GameBoard();
        public string[] WordsFound { get; set; } = [];
    }
}
