using Microsoft.AspNetCore.SignalR;
using WordSearch.Server.Models.GameLogic;

namespace WordSearch.Server.Services
{
    public interface ISingleplayerGame
    {
        public FindWordResultsForClient? FindWord(GameBoard gameBoard, (int, int) start, (int, int) direction, int count, string? userIdentifier);
        public GameBoard NewGame(Difficulty difficulty, string? userIdentifier = null);
    }
}
