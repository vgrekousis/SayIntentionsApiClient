namespace SayIntentions
{
    public class CommsHistoryResponse
    {
        public List<CommEntry> Comm_History { get; set; }
        public Mission Mission { get; set; }
        public ApiError Error { get; set; }
    }

}
