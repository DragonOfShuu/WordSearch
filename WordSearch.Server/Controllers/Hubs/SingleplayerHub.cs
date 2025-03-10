﻿using Microsoft.AspNetCore.SignalR;
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
                gameboard => {
                    Context.Items.Add(ANON_GAMEBOARD_ID, gameboard);
                    return gameboard.ToBoard();
                },
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
