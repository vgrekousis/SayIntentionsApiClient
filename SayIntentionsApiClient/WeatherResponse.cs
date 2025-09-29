namespace SayIntentions
{
    // === Weather ===
    public class WeatherResponse
    {
        public List<AirportWeather> Airports { get; set; }
        public List<CommFrequency> Comms { get; set; }
        public ApiError Error { get; set; }
    }

}
