namespace WordSearch.Server.Models.GameLogic
{
    public class GameBoard
    {
        Difficulty Difficulty { get; set; }
        string[][] BoardCharacters { get; set; }
        WordDictionary Findable { get; set; }
        string[] Found {  get; set; }
        int Started { get; } // milliseconds

        public static Board toBoard(GameBoard gameBoard)
        {

        }
    }

    public class Board
    {
        Difficulty Difficulty { get; set; }
        string[][] BoardCharacters { get; set; }
        string[][] Findable { get; set; }
        WordDictionary Found { get; set; }
        int Started { get; set; }
    }
}
