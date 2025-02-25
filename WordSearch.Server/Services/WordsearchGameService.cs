using Weighted_Randomizer;
using WordSearch.Server.Models.GameLogic;
using WordSearch.Server.Services.WordGenerator;

namespace WordSearch.Server.Services
{
    public class WordsearchGameService(IWordGenerator generator, ILogger<WordsearchGameService> logger) : IGameService
    {
        private readonly IWordGenerator _generator = generator;
        private readonly ILogger _logger = logger;

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
            Double difficultyLevel = difficulty.Level - 1;
            boardSize = (int)Math.Round(5 * Math.Log10(0.9 * (difficultyLevel) + 1) + 7.4);
            wordSize = (int)Math.Round(-8 * Math.Log10(0.1 * (difficultyLevel) + 0.5) + 5);
            wordCount = (int)Math.Round(7 * Math.Log10(0.07 * (difficultyLevel) + 0.1) + 11);
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
            List<int> weights = [];
            for (int i = minCount; i < maxCount+1; i++)
            {
                int weight = (int) -Math.Abs(i - favoredIndex) + 4;
                if (weight < 1)
                {
                    weight = 1;
                }
                randomizer.Add(i, weight);
                weights.Add(weight);
            }

            this._logger.LogDebug("Randomized indexes: {indexes}", randomizer);
            this._logger.LogDebug("Randomized weights: {weights}", weights);
            return randomizer;
        }

        /// <summary>
        /// Get a list of random words, dependent on the provided
        /// parameters.
        /// </summary>
        /// <param name="boardSize"></param>
        /// <param name="wordCount"></param>
        /// <param name="wordSize"></param>
        /// <returns></returns>
        private string[] GetWords(int boardSize, int wordSize, int wordCount)
        {
            Dictionary<int, int> lengths = [];
            var randomizer = SetupRandomizer(3, boardSize, wordSize);

            for (int i = 0; i < wordCount; i++)
            {
                int randomNum = randomizer.NextWithReplacement();
                //int defaultValue = 0;
                lengths.TryGetValue(randomNum, out int defaultValue);

                lengths[randomNum] = defaultValue+1;
            }

            this._logger.LogDebug("WordLengths: {lengths}", lengths);
            return this._generator.GetRandomWords([.. lengths]);
        }

        /// <summary>
        /// Generate a GameBoard based on the provided difficulty.
        /// </summary>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        public GameBoard generateGameBoard(Difficulty difficulty)
        {
            this._logger.LogDebug("Difficulty level: {difficulty}", difficulty.Level);
            GetWordsearchParams(difficulty, out int boardSize, out int wordSize, out int wordCount);
            this._logger.LogDebug("Boardsize: {boardSize}, wordSize: {wordSize}, wordCount: {wordCount}", boardSize, wordSize, wordCount);
            string[] words = GetWords(boardSize, wordSize, wordCount);
            
            var tempFindable = words
                .Select(word => new WordType() { Position = new Vector2D(), Rotation = new Vector2D(), Word = word })
                .ToDictionary(word => word.Word, word => word);

            return new GameBoard()
            {
                Difficulty = difficulty,
                BoardCharacters = [["a", "a"], ["a", "a"]],
                Findable = tempFindable,
                Found = [],
                Started = (new DateTimeOffset(DateTime.UtcNow)).ToUnixTimeMilliseconds()
            };
        }
    }
}
