using Microsoft.AspNetCore.SignalR;
using WordSearch.Server.Models.API;
using WordSearch.Server.Models.GameLogic;

namespace WordSearch.Server.Services
{
    public interface ISingleplayerGame
    {
        public Result<FindWordResultsForClient, APIError> FindWord(GameBoard gameBoard, (int, int) start, (int, int) direction, int count, string? userIdentifier);
        public Result<GameBoard, APIError> NewGame(Difficulty difficulty, string? userIdentifier = null);
    }
}
