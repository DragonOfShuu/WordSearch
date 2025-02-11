using Microsoft.AspNetCore.SignalR;
using WordSearch.Server.Models.GameLogic;

namespace WordSearch.Server.Services
{
    public interface ISingleplayerGame : IGameService
    {
        public GameBoard NewGame(Difficulty difficulty);
    }
}
