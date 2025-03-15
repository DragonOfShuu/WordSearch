using WordSearch.Server.Models.API;
using WordSearch.Server.Models.GameLogic;

namespace WordSearch.Server.Services
{
    public interface ISingleplayerGame
    {
        public Result<FindWordResultsSingleplayer, APIError>? FindWord(GameBoard gameBoard, Vector2D start, Vector2D direction, int count, string? userIdentifier);
        public Result<GameBoard, APIError> NewGame(Difficulty difficulty, string? userIdentifier = null);
    }

    public class FindWordResultsSingleplayer
    {
        public GameBoard Board { get; set; } = new GameBoard();
        public string[] WordsFound { get; set; } = [];
        public int XpGain { get; set; }
    }
}
