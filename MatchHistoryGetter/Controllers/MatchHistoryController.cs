using MatchHistoryGetter.Services;

namespace MatchHistoryGetter.Controllers
{
    public class MatchHistoryController
    {
        private readonly IRiotApiService _riotApiService;

        public MatchHistoryController(IRiotApiService riotService)
        {
            _riotApiService = riotService;
        }

        public void GetMatchHistory(string summonerName, string tagLine)
        {
            Console.WriteLine($"not yet implemented for {summonerName}#{tagLine}");
        }
    }
}
