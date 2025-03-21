using WordSearch.Server.Models.API;
using WordSearch.Server.Models.GameLogic;

namespace WordSearch.Server.Services
{
    public interface IGameService
    {
        Result<FindWordResults, APIError> FindWord(GameBoard gameBoard, Vector2D position, Vector2D direction, int count);
        Result<GameBoard, APIError> generateGameBoard(Difficulty difficulty, long? started = null);
    }
}
