using Microsoft.AspNetCore.SignalR;
using WordSearch.Server.Models.GameLogic;

namespace WordSearch.Server.Controllers.Hubs
{
    public class SingleplayerHub : Hub
    {
        public async Task<Board> NewGame(Difficulty difficulty)
        {

        }

        public async Task<Board> GetBoard()
        {

        }

        public async Task<WordDictionary> GetFoundWords()
        {
            string? indentifier = Context.UserIdentifier;
        }

        public async Task<FindWordResultsForClient?> FindWord((int, int) position, (int, int) rotation, int count)
        {

        }
    }
}
