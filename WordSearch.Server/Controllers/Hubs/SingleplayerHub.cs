using Microsoft.AspNetCore.SignalR;
using WordSearch.Server.Models.API;
using WordSearch.Server.Models.GameLogic;
using WordSearch.Server.Services;

namespace WordSearch.Server.Controllers.Hubs
{
    public class SingleplayerHub : Hub
    {
        private readonly ILogger _logger;
        private readonly ISingleplayerGame _singleplayerService;

        public SingleplayerHub(ILogger<SingleplayerHub> logger, ISingleplayerGame singleplayerGameService)
        {
            _logger = logger;
            _singleplayerService = singleplayerGameService;
        }

        public async Task<Board> NewGame(Difficulty difficulty)
        {
            Result<GameBoard, APIError> result = this._singleplayerService.NewGame(difficulty);
            return result.Match(
                gameboard => gameboard.ToBoard(),
                error => throw new HubException(error.Error));
        }

        public async Task<Board> GetBoard()
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, WordType>> GetFoundWords()
        {
            string? indentifier = Context.UserIdentifier;
            throw new NotImplementedException();
        }

        public async Task<FindWordResultsForClient?> FindWord((int, int) position, (int, int) rotation, int count)
        {
            throw new NotImplementedException();
        }
    }
}
