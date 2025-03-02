
namespace WordSearch.Server.Services.Utils
{
    public class RandomService : IRandomService
    {
        public Random Rand { get; }

        public RandomService()
        {
            Rand = new();
        }
    }
}
