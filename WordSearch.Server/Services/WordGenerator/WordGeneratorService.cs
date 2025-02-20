using WordSearch.Server.Services.WordGenerator;
using WordSearch.Server.Shared.JSONFileReader;

namespace WordSearch.Server.Services.WordSelector
{
    public class WordGeneratorService : IWordGenerator
    {
        private readonly ILogger<WordGeneratorService> _logger;
        public Dictionary<int, string[]> AllWords { get; set; }

        public WordGeneratorService(ILogger<WordGeneratorService> logger)
        {
            _logger = logger;

            // Hard coded for now, but I will most likely provide
            // some sort of language filter in the future if 
            // I ever decide to make multi-language support
            var words = JSONFileReader.Read<Dictionary<int, string[]>>("./Assets/WordLists/enWords.json");
            if (words == null)
            {
                throw new InvalidOperationException("Words.json is inaccessible; cannot generate words.");
            }
            AllWords = words;

            _logger.LogInformation("Successfully digested words.");
        }

        public string? getRandomWord(int length)
        {
            throw new NotImplementedException();
        }

        public string[] getRandomWords(params (int, int)[] words)
        {
            throw new NotImplementedException();
        }
    }
}
