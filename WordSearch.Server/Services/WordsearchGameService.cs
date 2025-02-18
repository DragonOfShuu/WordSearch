using WordSearch.Server.Models.GameLogic;

namespace WordSearch.Server.Services
{
    public class WordsearchGameService : IGameService
    {
        public FindWordResults? FindWord(GameBoard gameBoard, (int, int) position, (int, int) direction, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        public GameBoard generateGameBoard(Difficulty difficulty)
        {
            //throw new NotImplementedException();
            
            return new GameBoard()
            {
                Difficulty = difficulty,
                
            }
        }
    }
}
