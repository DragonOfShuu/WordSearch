using WordSearch.Server.Models.GameLogic;

namespace WordSearch.Server.Services
{
    public class WordsearchGameService : IGameService
    {
        public FindWordResults? FindWord(GameBoard gameBoard, (int, int) position, (int, int) direction, int count)
        {
            throw new NotImplementedException();
        }

        public GameBoard generateGameBoard(Difficulty difficulty)
        {
            throw new NotImplementedException();
        }
    }
}
