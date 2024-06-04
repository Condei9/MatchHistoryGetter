namespace MatchHistoryGetter.Models
{
    public class MetadataModel
    {
        public string DataVersion { get; set; }
        public string MatchId { get; set; }
        public List<string> Participants { get; set; }
    }
}
