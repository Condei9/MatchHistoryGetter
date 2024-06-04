using System.Reflection.Metadata.Ecma335;

namespace MatchHistoryGetter.Models
{
    public class MatchModel
    {
        public MetadataModel Metadata {  get; set; }
        public InfoModel Info { get; set; }
    }
}
