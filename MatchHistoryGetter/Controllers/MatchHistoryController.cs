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
            var matches = await _riotApiService.GetMatchHistoryAsync(summonerName, tagLine);
            var puuid = await _riotApiService.GetSummonerPuuId(summonerName, tagLine);

            foreach (var match in matches)
            {
                var participant = match.MatchInfo.Participants.FirstOrDefault(x => x.Puuid == puuid);
                Console.WriteLine();
            }
        }
    }
}
