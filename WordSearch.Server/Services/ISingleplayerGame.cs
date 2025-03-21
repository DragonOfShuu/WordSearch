using WordSearch.Server.Models.API;
using WordSearch.Server.Models.GameLogic;

namespace WordSearch.Server.Services
{
    public interface ISingleplayerGame
    {
        public Result<bool, APIError> FindWord(GameBoard gameBoard, Vector2D start, Vector2D direction, int count, Action<BoardUpdate> updateBoard, string? userIdentifier = null);
        public Result<GameBoard, APIError> NewGame(Difficulty difficulty, string? userIdentifier = null);
    }
}
