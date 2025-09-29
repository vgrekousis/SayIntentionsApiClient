namespace SayIntentions
{
    // === Error Handling ===
    public class ApiError
    {
        public string Error { get; set; }
        public string Status { get; set; } // Some errors return { "status": "UNKNOWN ERROR" }
    }

}
