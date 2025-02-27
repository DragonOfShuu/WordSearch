namespace WordSearch.Server.Models.API
{
    public class APIError
    {
        public string Error { get; set; } = string.Empty;

        public APIError(string error)
        {
            Error = error;
        }
    }
}
