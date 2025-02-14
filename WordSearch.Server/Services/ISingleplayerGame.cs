using Microsoft.AspNetCore.SignalR;
using WordSearch.Server.Models.GameLogic;

namespace WordSearch.Server.Services
{
    public interface ISingleplayerGame : IGameService
    {
        public FindWordResultsForClient? FindWord(GameBoard gameBoard, string? userIdentifier, (int, int) start, (int, int) direction, int count);
        public Board NewGame(Difficulty difficulty);
        public Board GetBoard(GameBoard gameBoard);
    }
}
