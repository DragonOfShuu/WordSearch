using WordSearch.Server.Services.WordGenerator;
using WordSearch.Server.Shared.JSONFileReader;

namespace WordSearch.Server.Services.WordSelector
{
    public class WordGenerator : IWordGenerator
    {
        private readonly ILogger<WordGenerator> _logger;
        public Dictionary<int, string[]> AllWords { get; set; }

        public WordGenerator(ILogger<WordGenerator> logger)
        {
            _logger = logger;

            var words = JSONFileReader.Read<Dictionary<int, string[]>>("./words.json");
            if (words == null)
            {
                throw new InvalidOperationException("Words.json is inaccessible; cannot generate words.");
            }
            AllWords = words;
        }

        public string getRandomWord(int length)
        {
            throw new NotImplementedException();
        }

        public string[] getRandomWords(params (int, int)[] words)
        {
            throw new NotImplementedException();
        }
    }
}
