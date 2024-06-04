namespace MatchHistoryGetter.Models
{
    public class InfoModel
    {
        public string EndOfGameResult { get; set; }
        public long GameCreation { get; set; }
        public long GameDuration { get; set; }
        public long? GameEndTimestamp { get; set; }
        public long GameId { get; set; }
        public string GameMode { get; set; }
        public string GameName { get; set; }
        public long GameStartTimestamp { get; set; }
        public string GameType { get; set; }
        public string GameVersion { get; set; }
        public int MapId { get; set; }
        public string PlatformId { get; set; }
        public int QueueId { get; set; }
        public List<ParticipantModel> Participants { get; set; } = new List<ParticipantModel>();
        public string TournamentCode { get; set; }
    }
}
