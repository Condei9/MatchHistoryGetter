namespace MatchHistoryGetter.Models
{
    public class MatchModel
    {
        public string GameId {  get; set; }
        public string Role { get; set; }
        public string Season { get; set; }
        public string PlatformId { get; set; }
        public string Champion { get; set; }
        public string Queue { get; set; }
        public string Lane { get; set; }
        public long Timestamp { get; set; }
    }
}
