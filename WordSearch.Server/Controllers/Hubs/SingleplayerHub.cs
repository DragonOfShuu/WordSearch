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

        }

        public async Task<Board?> FindWord((int, int) position, (int, int) rotation, int count)
        {

        }
    }
}
