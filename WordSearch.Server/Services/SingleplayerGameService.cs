using Microsoft.AspNetCore.SignalR;
using WordSearch.Server.Models.GameLogic;
using WordSearch.Server.Services.WordGenerator;

namespace WordSearch.Server.Services
{
    public class SingleplayerGameService : WordsearchGameService, ISingleplayerGame
    {
        private readonly IWordGenerator _wordGenerator;

        public SingleplayerGameService(IWordGenerator wordGenerator)
        {
            _wordGenerator = wordGenerator;
        }

        public FindWordResultsForClient? FindWord(GameBoard gameBoard, (int, int) start, (int, int) direction, int count, string? userIdentifier)
        {
            throw new NotImplementedException();
        }

        public GameBoard NewGame(Difficulty difficulty, string? userIdentifier)
        {
            return generateGameBoard(difficulty);
        }
    }
}
