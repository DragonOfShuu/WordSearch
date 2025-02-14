using Microsoft.AspNetCore.SignalR;
using WordSearch.Server.Models.GameLogic;

namespace WordSearch.Server.Controllers.Hubs
{
    public class SingleplayerHub : Hub
    {
        private readonly ILogger _logger;

        public SingleplayerHub(ILogger<SingleplayerHub> logger)
        {
            _logger = logger;
        }

        public async Task<Board> NewGame(Difficulty difficulty)
        {
            return new Board
            {
                BoardCharacters = [],
                Difficulty = difficulty,
                Findable = ["Hello", "World"],
                Found = [],
                Started = 0
            };
        }

        public async Task<Board> GetBoard()
        {
            throw new NotImplementedException();
        }

        public async Task<WordDictionary> GetFoundWords()
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
