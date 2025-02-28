﻿using System.Text.Json;
using Weighted_Randomizer;
using WordSearch.Server.Models.API;
using WordSearch.Server.Models.GameLogic;
using WordSearch.Server.Services.WordGenerator;

namespace WordSearch.Server.Services
{
    public class WordsearchGameService(IWordGenerator generator, ILogger<WordsearchGameService> logger) : IGameService
    {
        private readonly IWordGenerator _generator = generator;
        private readonly ILogger _logger = logger;

        public Result<FindWordResults, APIError> FindWord(GameBoard gameBoard, (int, int) position, (int, int) direction, int count)
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
        /// <param name="sizeX">The x size of the board</param>
        /// <param name="sizeY">The y size of the board</param>
        /// <param name="wordSize">The majority size of the words</param>
        /// <param name="wordCount"></param>
        private void GetWordsearchParams(Difficulty difficulty, out int sizeX, out int sizeY, out int wordSize, out int wordCount)
        {
            Double difficultyLevel = difficulty.Level - 1;
            sizeX = sizeY = (int)Math.Round(5 * Math.Log10(0.9 * (difficultyLevel) + 1) + 7.4);
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

            return randomizer;
        }

        /// <summary>
        /// Get a list of random words, dependent on the provided
        /// parameters.
        /// </summary>
        /// <param name="maxSize"></param>
        /// <param name="wordCount"></param>
        /// <param name="wordSize"></param>
        /// <returns></returns>
        private string[] GetWords(int maxSize, int wordSize, int wordCount)
        {
            Dictionary<int, int> lengths = [];
            var randomizer = SetupRandomizer(3, maxSize, wordSize);

            for (int i = 0; i < wordCount; i++)
            {
                int randomNum = randomizer.NextWithReplacement();
                //int defaultValue = 0;
                lengths.TryGetValue(randomNum, out int defaultValue);

                lengths[randomNum] = defaultValue+1;
            }

            return _generator.GetRandomWords([.. lengths]);
        }

        private Vector2D[]? TestWordPlaceability(string word, Transform trans, string[][] wordsearch)
        {
            List<Vector2D> wordCoords = [];
            for (int alongWord = 0; alongWord < word.Length; alongWord++)
            {
                int xPos = trans.Position.X + (alongWord * trans.Rotation.X);
                int yPos = trans.Position.Y + (alongWord * trans.Rotation.Y);

                //_logger.LogError("Coords: {x},{y}", xPos, yPos);
                if (wordsearch[yPos][xPos] == null) continue;

                if (wordsearch[yPos][xPos] == word[alongWord].ToString()) continue;

                // The letter cannot be placed here,
                // thus the word cannot be placed
                return null;
            }

            return [.. wordCoords];
        }

        private class PlaceWordsResult
        {
            public Dictionary<string, WordType> findable = [];
            public string[][] wordsearch = new string[0][];
        }

        private PlaceWordsResult? PlaceWordsearchWords(
            string[] remainingWords,
            Dictionary<string, WordType> placedWords, 
            string[][] wordsearch,
            int boardX,
            int boardY
        )
        {
            if (remainingWords.Length == 0)
            {
                return new PlaceWordsResult() { 
                    findable = placedWords, 
                    wordsearch = wordsearch 
                };
            }

            string newWord = remainingWords[0];
            PositionTable posTable = new(boardX, boardY, newWord.Length);
            //_logger.LogDebug("posTable: {table}", posTable.Table);
            while (!posTable.IsEmpty())
            {
                var trans = posTable.RandomEject();
                if (trans == null) return null;

                Vector2D[]? placeWordCoords = TestWordPlaceability(newWord, trans, wordsearch);
                if (placeWordCoords == null) continue;

                string[][] newWordsearch = (string[][]) wordsearch.Clone();

                for (int i = 0; i < placeWordCoords.Length; i++)
                {
                    var coord = placeWordCoords[i];
                    newWordsearch[coord.Y][coord.X] = newWord[i].ToString();
                }

                placedWords.Add(newWord, new WordType()
                {
                    Position = trans.Position,
                    Rotation = trans.Rotation,
                    Word = newWord
                });
                _logger.LogDebug(JsonSerializer.Serialize(newWordsearch));
                PlaceWordsResult? placeResults = PlaceWordsearchWords(remainingWords.Skip(1).ToArray(), placedWords, newWordsearch, boardX, boardY);
                if (placeResults != null) return placeResults;
                
                // Remove the word, since it does not work
                placedWords.Remove(newWord);
            }

            return null;
        }

        private string[][] CreateWordsearchBoard(int sizeX, int sizeY)
        {
            string[][] table = new string[sizeY][];
            for (int i = 0;i < sizeY;i++)
            {
                table[i] = new string[sizeX];
            }
            return table;
        }

        /// <summary>
        /// Generate a GameBoard based on the provided difficulty.
        /// </summary>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        public Result<GameBoard, APIError> generateGameBoard(Difficulty difficulty)
        {
            GetWordsearchParams(difficulty, out int sizeX, out int sizeY, out int wordSize, out int wordCount);
            _logger.LogDebug("BoardSize: {size}, wordSize: {wordSize}, wordCount: {wordCount}", (sizeX, sizeY), wordSize, wordCount);

            int attempts = 0;
            PlaceWordsResult? placeResults = null;
            while (attempts < 100 && placeResults == null)
            {
                if (attempts % 5 == 0 && attempts != 0)
                {
                    _logger.LogWarning("Attempted setting up a board {attempts} times!", attempts);
                }
                string[] words = GetWords(Math.Min(sizeX, sizeY), wordSize, wordCount);
                placeResults = PlaceWordsearchWords(words, [], CreateWordsearchBoard(sizeX, sizeY), sizeX, sizeY);
                attempts++;
            }

            if (placeResults == null) return new APIError("Attempt limit reached on wordsearch generation.");
            
            return new GameBoard()
            {
                Difficulty = difficulty,
                BoardCharacters = placeResults.wordsearch,
                Findable = placeResults.findable,
                Found = [],
                Started = (new DateTimeOffset(DateTime.UtcNow)).ToUnixTimeMilliseconds()
            };
        }
    }
}
