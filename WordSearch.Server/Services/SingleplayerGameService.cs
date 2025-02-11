using Microsoft.AspNetCore.SignalR;
using WordSearch.Server.Models.GameLogic;

namespace WordSearch.Server.Services
{
    public class SingleplayerGameService : WordsearchGameService, ISingleplayerGame
    {
        public GameBoard? FindWord(HubCallerContext context, (int, int) start, (int, int) direction, int count)
        {
            throw new NotImplementedException();
        }

        public Board GetBoard(HubCallerContext context)
        {
            throw new NotImplementedException();
        }

        public GameBoard NewGame(Difficulty difficulty)
        {
            throw new NotImplementedException();
        }
    }
}
