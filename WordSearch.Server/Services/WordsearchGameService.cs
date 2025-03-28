﻿using System.Text;
using System.Text.Json;
using Weighted_Randomizer;
using WordSearch.Server.Models.API;
using WordSearch.Server.Models.GameLogic;
using WordSearch.Server.Services.Utils;
using WordSearch.Server.Services.WordGenerator;
using WordSearch.Server.Shared.PerfTimer;

namespace WordSearch.Server.Services
{
    public class WordsearchGameService(IWordGenerator generator, ILogger<WordsearchGameService> logger, IRandomService random) : IGameService
    {
        private readonly IWordGenerator _generator = generator;
        private readonly ILogger _logger = logger;
        private readonly IRandomService _random = random;

        private string DiscoverWord(string[][] boardCharacters, Vector2D position, Vector2D direction, int count)
        {
            var word = new StringBuilder(count);
            for (int counter = 0; counter < count; counter++)
            {
                var newX = (counter*direction.X) + position.X;
                var newY = (counter*direction.Y) + position.Y;

                var letter = boardCharacters[newY][newX];
                word.Append(letter);
            }
            return word.ToString();
        }

        public Result<FindWordResults, APIError> FindWord(GameBoard gameBoard, Vector2D position, Vector2D direction, int count)
        {
            var word = DiscoverWord(gameBoard.BoardCharacters, position, direction, count);
            gameBoard.Findable.TryGetValue(word, out var wordType);
            if (wordType == null) return new FindWordResults()
            {
                GameBoard = gameBoard,
                WordsFound = [],
            };

            gameBoard.Found = [..gameBoard.Found, word];

            return new FindWordResults()
            {
                GameBoard = gameBoard,
                WordsFound = [word]
            };
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
            sizeX = sizeY = (int)Math.Round(5 * Math.Log10(0.5 * (difficultyLevel) + 1) + 7.4);
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

            var adjustedWordSize = wordSize < 3 ? 3 : (wordSize > maxSize ? maxSize : wordSize);
            var randomizer = SetupRandomizer(3, maxSize, adjustedWordSize);

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

                if (wordsearch[yPos][xPos] != null && wordsearch[yPos][xPos] != word[alongWord].ToString()) 
                    return null;

                wordCoords.Add(new Vector2D() { X = xPos, Y = yPos });
            }

            return [.. wordCoords];
        }

        private class PlaceWordsSuccess
        {
            public SortedDictionary<string, WordType> Findable { get; set; } = [];
            public string[][] Wordsearch { get; set; } = [];
        }

        private class PlaceWordsFail
        {
            public string Reason { get; set; } = string.Empty;
        }

        private Result<PlaceWordsSuccess, PlaceWordsFail> PlaceWordsearchWords(
            string[] remainingWords,
            SortedDictionary<string, WordType> placedWords, 
            string[][] wordsearch,
            int boardX,
            int boardY,
            IPerfTimer timer
        )
        {
            if (remainingWords.Length == 0)
            {
                return new PlaceWordsSuccess() { 
                    Findable = placedWords, 
                    Wordsearch = wordsearch 
                };
            }

            string newWord = remainingWords[0];
            PositionTable posTable = new(boardX, boardY, newWord.Length);
            while (!posTable.IsEmpty() && timer.IsValid())
            {
                var lastRotation = placedWords.Count > 0 ? placedWords.Last().Value.Rotation : null;
                var trans = posTable.RandomEject(lastRotation == null ? [] : [lastRotation, lastRotation.Change(v=> v*-1)]);
                if (trans == null) return new PlaceWordsFail() { Reason = "Cannot place" };

                Vector2D[]? placeWordCoords = TestWordPlaceability(newWord, trans, wordsearch);
                if (placeWordCoords == null) continue;

                string[][] newWordsearch = wordsearch.Select(inner => (string[]) inner.Clone()).ToArray();

                for (int i = 0; i < placeWordCoords.Length; i++)
                {
                    var coord = placeWordCoords[i];
                    newWordsearch[coord.Y][coord.X] = newWord[i].ToString();
                }

                placedWords.TryAdd(newWord, new WordType()
                {
                    Position = trans.Position,
                    Rotation = trans.Rotation,
                    Word = newWord
                });
                Result<PlaceWordsSuccess, PlaceWordsFail> placeResults = PlaceWordsearchWords(remainingWords.Skip(1).ToArray(), placedWords, newWordsearch, boardX, boardY, timer);

                if (placeResults.IsOk)
                    return placeResults;
                
                // Remove the word, since it does not work
                placedWords.Remove(newWord);
            }

            return new PlaceWordsFail() { Reason = timer.IsValid() ? "Cannot place" : "Ran out of time" };
        }

        private string[][] FillWithTrash(string[][] wordsearch)
        {
            string chars = "abcdefghijklmnopqrstuvwxyz";
            return wordsearch.Select(row => row.Select(potChar =>
            {
                if (potChar != null) return potChar;
                return chars[_random.Rand.Next(chars.Length)].ToString();
            }).ToArray()).ToArray();
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
        public Result<GameBoard, APIError> generateGameBoard(Difficulty difficulty, long? started = null)
        {
            GetWordsearchParams(difficulty, out int sizeX, out int sizeY, out int wordSize, out int wordCount);

            int attempts = 0;
            PlaceWordsSuccess? placeResults = null;
            while (attempts < 5 && placeResults == null)
            {
                attempts++;
                if (attempts % 5 == 0 && attempts != 0)
                {
                    _logger.LogWarning("Attempted setting up a board {attempts} times!", attempts);
                }
                string[] words = GetWords(Math.Min(sizeX, sizeY), wordSize, wordCount);
                //_logger.LogDebug("Words {words}\nWordCount {wordCount}", words, wordCount);
                PerfTimer timer = new(500);
                timer.Start();
                var potentialResults = PlaceWordsearchWords(words, [], CreateWordsearchBoard(sizeX, sizeY), sizeX, sizeY, timer);
                timer.Reset();

                placeResults = potentialResults.Match<PlaceWordsSuccess?>(
                    success => success,
                    failure => null
                );
            }

            if (placeResults == null) return new APIError("Attempt limit reached on wordsearch generation.");
            
            return new GameBoard()
            {
                Difficulty = difficulty,
                BoardCharacters = FillWithTrash(placeResults.Wordsearch),
                //BoardCharacters = placeResults.Wordsearch,
                Findable = placeResults.Findable.ToDictionary(),
                Found = [],
                Started = started==null ? (new DateTimeOffset(DateTime.UtcNow)).ToUnixTimeMilliseconds() : (long) started
            };
        }
    }
}
