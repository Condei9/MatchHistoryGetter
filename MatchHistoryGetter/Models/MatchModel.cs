using System.Reflection.Metadata.Ecma335;

namespace MatchHistoryGetter.Models
{
    public class MatchModel
    {
        public MetadataModel MetadataInfo {  get; set; } = new MetadataModel();
        public InfoModel MatchInfo { get; set; } = new InfoModel();
    }
}
