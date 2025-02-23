using Weighted_Randomizer;
using WordSearch.Server.Models.GameLogic;
using WordSearch.Server.Services.WordGenerator;

namespace WordSearch.Server.Services
{
    public class WordsearchGameService : IGameService
    {
        private readonly IWordGenerator _generator;

        public WordsearchGameService(IWordGenerator generator)
        {
            _generator = generator;
        }

        public FindWordResults? FindWord(GameBoard gameBoard, (int, int) position, (int, int) direction, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the corresponding values based
        /// on the provided difficulty.
        /// 
        /// All nummbers are calculated by the
        /// difficultyLevel, with a logarithmic
        /// equation.
        /// </summary>
        /// <param name="difficulty">The difficulty level of the parameters.</param>
        /// <param name="boardSize">The size of the board</param>
        /// <param name="wordSize">The majority size of the words</param>
        /// <param name="wordCount"></param>
        private void GetWordsearchParams(Difficulty difficulty, out int boardSize, out int wordSize, out int wordCount)
        {
            Double difficultyLevel = difficulty.Level;
            boardSize = (int)Math.Round(5d * Math.Log(0.9d * (difficultyLevel) + 1d) + 7.4);
            wordSize = (int)Math.Round(-8d * Math.Log(0.1d * (difficultyLevel) + 0.5) + 5d);
            wordCount = (int)Math.Round(7d * Math.Log(0.07d * (difficultyLevel) + 0.1) + 11d);
        }

        /// <summary>
        /// Create a static randomizer that chooses a random
        /// number, with an index that is more likely (along
        /// with the neighboring numbers more likely with a 
        /// radius of 4)
        /// </summary>
        /// <param name="minCount">Inclusive</param>
        /// <param name="maxCount">Inclusive</param>
        /// <param name="favoredIndex">The index that is more likely to be chosen</param>
        /// <returns></returns>
        private IWeightedRandomizer<int> SetupRandomizer(int minCount, int maxCount, int favoredIndex)
        {
            IWeightedRandomizer<int> randomizer = new StaticWeightedRandomizer<int>();
            for (int i = minCount; i < maxCount+1; i++)
            {
                int weight = (int) Math.Round(-0.25 * Math.Pow(i - favoredIndex, 2) + 4);
                if (weight < 1)
                {
                    weight = 1;
                }
                randomizer.Add(i, weight);
            }

            return randomizer;
        }

        /// <summary>
        /// Get a list of random words, dependent on the provided
        /// parameters.
        /// </summary>
        /// <param name="BoardSize"></param>
        /// <param name="WordCount"></param>
        /// <param name="WordSize"></param>
        /// <returns></returns>
        private string[] GetWords(int BoardSize, int WordCount, int WordSize)
        {
            Dictionary<int, int> lengths = [];
            var randomizer = SetupRandomizer(3, BoardSize, WordSize);

            for (int i = 0; i < WordCount; i++)
            {
                int randomNum = randomizer.NextWithReplacement();
                //int defaultValue = 0;
                lengths.TryGetValue(randomNum, out int defaultValue);

                lengths[randomNum] = defaultValue+1;
            }

            return this._generator.GetRandomWords([.. lengths]);
        }

        /// <summary>
        /// Generate a GameBoard based on the provided difficulty.
        /// </summary>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        public GameBoard generateGameBoard(Difficulty difficulty)
        {
            GetWordsearchParams(difficulty, out int boardSize, out int wordSize, out int wordCount);
            string[] words = GetWords(boardSize, wordSize, wordCount);

            return new GameBoard()
            {
                Difficulty = difficulty,
                BoardCharacters = [],
                Findable = new WordDictionary(),
                Found = [],
                Started = (new DateTimeOffset(DateTime.UtcNow)).ToUnixTimeMilliseconds()
            };
        }
    }
}
