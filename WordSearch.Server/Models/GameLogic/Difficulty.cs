namespace WordSearch.Server.Models.GameLogic
{
    public class Difficulty
    {
        int Level { get; set; }
        string Intensity { get; set; } // medium
        (int, int) Size { get; set; }
        int Time { get; set; } // in seconds
    }
}
