using Microsoft.AspNetCore.SignalR;
using WordSearch.Server.Models.API;
using WordSearch.Server.Models.GameLogic;
using WordSearch.Server.Services.WordGenerator;

namespace WordSearch.Server.Services
{
    public class SingleplayerGameService(IGameService wordSearch) : ISingleplayerGame
    {
        private readonly IGameService _wordsearch = wordSearch;

        public Result<FindWordResultsSingleplayer, APIError>? FindWord(
            GameBoard gameBoard, 
            Vector2D start, 
            Vector2D direction, 
            int count, 
            string? userIdentifier)
        {
            var endTime = gameBoard.Started + gameBoard.Difficulty.Time * 1000;
            if (endTime < (new DateTimeOffset(DateTime.UtcNow)).ToUnixTimeMilliseconds())
                return null;

            var findResults = _wordsearch.FindWord(gameBoard, start, direction, count);

            if (!findResults.IsOk) return findResults.Error;

            int xpGain = findResults.Value.WordsFound.Length * 5;

            return new FindWordResultsSingleplayer
            {
                WordsFound = findResults.Value.WordsFound,
                Board = findResults.Value.GameBoard,
                XpGain = xpGain,
            };
        }

        public Result<GameBoard, APIError> NewGame(Difficulty difficulty, string? userIdentifier)
        {
            return _wordsearch.generateGameBoard(difficulty);
        }
    }
}
