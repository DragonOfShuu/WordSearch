using WordSearch.Server.Models.API;
using WordSearch.Server.Models.GameLogic;

namespace WordSearch.Server.Services
{
    public interface IGameService
    {
        Result<FindWordResults, APIError> FindWord(GameBoard gameBoard, (int, int) position, (int, int) direction, int count);
        Result<GameBoard, APIError> generateGameBoard(Difficulty difficulty);
    }
}
