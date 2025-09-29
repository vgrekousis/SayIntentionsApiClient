namespace SayIntentions
{
    public class CurrentFrequenciesResponse
    {
        public Dictionary<string, string> Frequencies { get; set; }
        public ApiError Error { get; set; }
    }

}
