using Microsoft.AspNetCore.SignalR;

namespace WordSearch.Server.Controllers.Hubs
{
    public class MultiplayerHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
