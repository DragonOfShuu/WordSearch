using WordSearch.Server.Models.GameLogic;

namespace WordSearch.Server.Services
{
    public interface IGameService
    {
        GameBoard? FindWord(GameBoard gameBoard, (int, int) position, (int, int) direction, int count);
        GameBoard generateGameBoard(Difficulty difficulty);
    }
}
