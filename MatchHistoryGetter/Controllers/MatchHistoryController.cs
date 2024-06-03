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

        public async Task GetMatchHistory(string summonerName, string tagLine)
        {
            try
            {
                var matches = await _riotApiService.GetMatchHistoryAsync(summonerName, tagLine);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"not yet implemented for {summonerName}#{tagLine}");
            }
        }
    }
}
