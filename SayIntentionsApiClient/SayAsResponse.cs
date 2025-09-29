namespace SayIntentions
{
    // === Communication ===
    public class SayAsResponse
    {
        public string Message { get; set; }
        public string Channel { get; set; }
        public bool Success => string.IsNullOrEmpty(Error?.Error);
        public ApiError Error { get; set; }
    }

}
