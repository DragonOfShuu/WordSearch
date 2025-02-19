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
        public string[] getRandomWords(params (int, int)[] words);
        /// <summary>
        /// Get a random word of a specified length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string getRandomWord(int length);
    }
}
