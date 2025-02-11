namespace WordSearch.Server.Models.GameLogic
{
    public class GameBoard
    {
        Difficulty Difficulty { get; set; }
        string[][] Board { get; set; }
        WordDictionary Findable { get; set; }
        string[] found {  get; set; }
        int started { get; } // milliseconds
    }
}
