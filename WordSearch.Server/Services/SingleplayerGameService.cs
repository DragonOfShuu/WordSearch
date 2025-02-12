using Microsoft.AspNetCore.SignalR;
using WordSearch.Server.Models.GameLogic;

namespace WordSearch.Server.Services
{
    public class SingleplayerGameService : WordsearchGameService, ISingleplayerGame
    {
        public FindWordResultsForClient? FindWord(GameBoard gameBoard, string? userIdentifier, (int, int) start, (int, int) direction, int count)
        {
            throw new NotImplementedException();
        }

        public Board GetBoard(GameBoard gameBoard)
        {
            throw new NotImplementedException();
        }

        public GameBoard NewGame(Difficulty difficulty)
        {
            throw new NotImplementedException();
        }
    }
}
