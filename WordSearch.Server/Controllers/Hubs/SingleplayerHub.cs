using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using WordSearch.Server.Models.API;
using WordSearch.Server.Models.GameLogic;
using WordSearch.Server.Services;

namespace WordSearch.Server.Controllers.Hubs
{
    public class SingleplayerHub : Hub
    {
        private readonly ILogger _logger;
        private readonly ISingleplayerGame _singleplayerService;
        private const string ANON_GAMEBOARD_ID = "AnonGameboard";

        private GameBoard? SavedBoard 
        {
            get
            {
                Context.Items.TryGetValue(ANON_GAMEBOARD_ID, out object? outObject);
                if (outObject == null) return null;
                return (GameBoard) outObject;
            }
            set
            {
                Context.Items.Add(ANON_GAMEBOARD_ID, value);
            }
        }

        public SingleplayerHub(ILogger<SingleplayerHub> logger, ISingleplayerGame singleplayerGameService)
        {
            _logger = logger;
            _singleplayerService = singleplayerGameService;
        }

        public async Task<Board> NewGame(Difficulty difficulty)
        {
            var sw = new Stopwatch();
            sw.Start();
            Result<GameBoard, APIError> result = _singleplayerService.NewGame(difficulty);
            sw.Stop();
            _logger.LogDebug("Elapsted time on gen: {elapsed}ms", sw.ElapsedMilliseconds);

            // Removes one if there is one
            Context.Items.Remove(ANON_GAMEBOARD_ID);
            return result.Match(
                gameboard => (SavedBoard = gameboard).ToBoard(),
                error => throw new HubException(error.Error));
        }

        public async Task<Board> GetBoard()
        {
            if (SavedBoard is null) throw new HubException("No boards are saved!");

            return SavedBoard.ToBoard();
        }

        public async Task<Dictionary<string, WordType>> GetFoundWords()
        {
            if (SavedBoard is null) throw new HubException("No boards are saved!");

            return SavedBoard.FoundAsWordType();
        }

        public async Task<FindWordResultsForClient?> FindWord(Vector2D position, Vector2D rotation, int count)
        {
            if (SavedBoard is null) throw new HubException("No boards are saved!");

            Result<FindWordResultsForClient, APIError>? result = 
                _singleplayerService.FindWord(SavedBoard, position, rotation, count, null);

            return new FindWordResultsForClient()
            {
                Board = SavedBoard.ToBoard(),
                WordsFound = ["word"],
                XpGain = 5,
            };
        }
    }
}
