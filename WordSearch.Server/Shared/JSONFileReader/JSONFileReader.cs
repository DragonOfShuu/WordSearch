using System.Text.Json;

namespace WordSearch.Server.Shared.JSONFileReader
{
    public class JSONFileReader
    {
        public static T? Read<T>(string filePath)
        {
            string text = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(text);
        }
    }
}
