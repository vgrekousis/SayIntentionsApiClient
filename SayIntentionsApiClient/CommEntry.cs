namespace SayIntentions
{
    public class CommEntry
    {
        public int Id { get; set; }
        public string Ident { get; set; }
        public int Copilot { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Frequency { get; set; }
        public int Is_Acars { get; set; }
        public string Channel { get; set; }
        public DateTime Stamp_Zulu { get; set; }
        public string Station_Name { get; set; }
        public string Outgoing_Message { get; set; }
        public string Incoming_Message { get; set; }
        public string Atc_Url { get; set; }
        public string Pilot_Url { get; set; }
    }

}
