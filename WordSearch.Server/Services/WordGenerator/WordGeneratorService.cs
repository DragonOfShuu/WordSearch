using WordSearch.Server.Services.WordGenerator;
using WordSearch.Server.Shared.JSONFileReader;

namespace WordSearch.Server.Services.WordSelector
{
    public class WordGeneratorService : IWordGenerator
    {
        private readonly ILogger<WordGeneratorService> _logger;
        public Dictionary<int, string[]> AllWords { get; set; }
        
        private readonly Random random = new Random();

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

        public string? GetRandomWord(int length)
        {
            var words = AllWords[length];
            if (words == null) return null;

            var randomIndex = this.random.Next(words.Length);
            return words[randomIndex];
        }

        /// <summary>
        /// Get random words depending on a KeyValuePair
        /// list of length,count.
        /// </summary>
        /// <param name="wordParameters"></param>
        /// <returns></returns>
        public string[] GetRandomWords(KeyValuePair<int, int>[] wordParameters)
        {
            List<string> result = [];
            
            foreach (var item in wordParameters)
            {
                (int length, int count) = item;

                string[]? words = GetRandomWords(count, length);

                if (words == null) continue;

                result.AddRange(words);
            }

            return [.. result];
        }

        public string[]? GetRandomWords(int count, int length)
        {
            List<string> result = [];

            for (int i = 0; i < count; i++)
            {
                string? randomWord = GetRandomWord(length);

                if (randomWord == null) return null;

                result.Add(randomWord);
            }

            return [.. result];
        }
    }
}
