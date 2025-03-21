using Microsoft.AspNetCore.SignalR;
using WordSearch.Server.Models.API;
using WordSearch.Server.Models.GameLogic;
using WordSearch.Server.Services.WordGenerator;

namespace WordSearch.Server.Services
{
    public class SingleplayerGameService(IGameService wordSearch) : ISingleplayerGame
    {
        private readonly IGameService _wordsearch = wordSearch;
        private readonly Dictionary<int, GameBoard> _games = [];

        public Result<bool, APIError> FindWord(
            GameBoard gameBoard,
            Vector2D start,
            Vector2D direction,
            int count,
            Action<BoardUpdate> updateBoard,
            string? userIdentifier)
        {
            var endTime = gameBoard.Started + gameBoard.Difficulty.Time * 1000;
            if (endTime < (new DateTimeOffset(DateTime.UtcNow)).ToUnixTimeMilliseconds())
                return false;

            var findResults = _wordsearch.FindWord(gameBoard, start, direction, count);

            if (!findResults.IsOk) return findResults.Error;

            var adjustedGameboard = findResults.Value.GameBoard;
            var wordsFound = adjustedGameboard.AllWordsFound();

            bool needToCreateAnotherBoard = true;
            GameBoard? newBoard = null;
            if (wordsFound)
            {
                _games.TryGetValue(adjustedGameboard.Difficulty.IncreaseDifficulty().Level, out newBoard);
                if (newBoard == null)
                {
                    var boardResult = GenerateNext(2, adjustedGameboard.Difficulty.IncreaseDifficulty(), adjustedGameboard.Started);
                    if (!boardResult.IsOk) return boardResult.Error;
                    needToCreateAnotherBoard = false;
                    newBoard = boardResult.Value;
                }
            }

            int xpGain = findResults.Value.WordsFound.Length * 5 * (wordsFound ? 3 : 1);

            updateBoard(new BoardUpdate()
            {
                Board = adjustedGameboard,
                WordsFound = findResults.Value.WordsFound,
                XpGain = xpGain,
                NewBoard = newBoard
            });

            if (needToCreateAnotherBoard && newBoard != null)
            {
                GenerateNext(1, newBoard.Difficulty.IncreaseDifficulty(), newBoard.Started);
            }

            return true;
        }

        public Result<GameBoard, APIError> NewGame(Difficulty difficulty, string? userIdentifier)
        {
            if (_games.Count > 0)
            {
                _games.Clear();
            }

            var newBoardResult = _wordsearch.generateGameBoard(difficulty);
            var nextBoardResult = _wordsearch.generateGameBoard(difficulty.IncreaseDifficulty());

            if (!newBoardResult.IsOk) return newBoardResult;
            if (!nextBoardResult.IsOk) return nextBoardResult;

            var newBoard = newBoardResult.Value;
            var nextBoard = nextBoardResult.Value;

            _games.Add(nextBoard.Difficulty.Level, nextBoard);

            return newBoard;
        }

        public Result<GameBoard, APIError> GenerateNext(int count, Difficulty startingDifficulty, long? started=null, int steps = 1)
        {
            if (count < 1) return new APIError("Not generating any gameboards");
            var firstResult = _wordsearch.generateGameBoard(startingDifficulty, started);
            if (!firstResult.IsOk) return firstResult;

            var firstGameboard = firstResult.Value;
            var escalatedDifficulty = startingDifficulty.IncreaseDifficulty();
            Dictionary<int, GameBoard> gameByLevel = [];
            for (var i = 0; i < count - 1; i++)
            {
                var result = _wordsearch.generateGameBoard(escalatedDifficulty, started);
                if (!result.IsOk) return result;
                gameByLevel.Add(escalatedDifficulty.Level, result.Value);
            }

            foreach (var newGame in  gameByLevel)
            {
                _games.Add(newGame.Key, newGame.Value);
            }

            return firstGameboard;
        }
    }
}
