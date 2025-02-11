namespace WordSearch.Server.Models.GameLogic
{
    public class WordDictionary : Dictionary<string, WordType>
    {

    }

    public class WordType
    {
        string Word { get; set; }
        (int, int) Position { get; set; }
        (int, int) Rotation { get; set; }
    }
}
