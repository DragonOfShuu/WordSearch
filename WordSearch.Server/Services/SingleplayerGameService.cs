using Microsoft.AspNetCore.SignalR;
using WordSearch.Server.Models.API;
using WordSearch.Server.Models.GameLogic;
using WordSearch.Server.Services.WordGenerator;

namespace WordSearch.Server.Services
{
    public class SingleplayerGameService(IGameService wordSearch) : ISingleplayerGame
    {
        private readonly IGameService _wordsearch = wordSearch;

        public Result<FindWordResultsForClient, APIError> FindWord(GameBoard gameBoard, (int, int) start, (int, int) direction, int count, string? userIdentifier)
        {
            throw new NotImplementedException();
        }

        public Result<GameBoard, APIError> NewGame(Difficulty difficulty, string? userIdentifier)
        {
            return this._wordsearch.generateGameBoard(difficulty);
        }
    }
}
