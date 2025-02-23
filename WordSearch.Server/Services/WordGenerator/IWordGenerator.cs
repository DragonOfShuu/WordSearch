namespace WordSearch.Server.Services.WordGenerator
{
    public interface IWordGenerator
    {
        /// <summary>
        /// Return a list of words that have the corresponding
        /// length and number of that length.
        /// </summary>
        /// <param name="words">
        /// (count, length): The amount of words 
        /// plus the count
        /// </param>
        /// <returns></returns>
        public string[] GetRandomWords(KeyValuePair<int, int>[] words);
        /// <summary>
        /// Get a list of words that have the corresponding
        /// quantity and length.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string[]? GetRandomWords(int count, int length);
        /// <summary>
        /// Get a random word of a specified length
        /// </summary>
        /// <param name="length"></param>
        /// <returns>The word, or null if that length does not exist.</returns>
        public string? GetRandomWord(int length);
    }
}
