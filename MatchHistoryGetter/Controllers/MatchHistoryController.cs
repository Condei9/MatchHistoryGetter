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

        public async Task GetStatsAndMatchHistory(string summonerName, string tagLine)
        {
            var matches = await _riotApiService.GetMatchHistoryAsync(summonerName, tagLine);
            var puuid = await _riotApiService.GetSummonerPuuId(summonerName, tagLine);

            var startTime24Hours = DateTime.UtcNow.AddDays(-1);
            long startTimeUnix24Hours = new DateTimeOffset(startTime24Hours).ToUnixTimeMilliseconds();

            var wins7Days = 0;
            var games7Days = matches.Count;

            var wins24Hours = 0;
            var games24Hours = 0;

            var totalKills7Days = 0;
            var totalDeaths7Days = 0;
            var totalAssists7Days = 0;

            var totalKills24Hours = 0;
            var totalDeaths24Hours = 0;
            var totalAssists24Hours = 0;

            Console.WriteLine("\nChampionName    Kills/Deaths/Assists      Result");
            Console.WriteLine("----------------------------------------------------");

            foreach (var match in matches)
            {
                var participant = match.MatchInfo.Participants.FirstOrDefault(x => x.Puuid == puuid);
                Console.WriteLine($"{participant.ChampionName}           {participant.Kills}/{participant.Deaths}/{participant.Assists}                 Result: {participant.Win}");

                if(participant.Win)
                {
                    wins7Days++;
                }

                totalKills7Days += participant.Kills;
                totalDeaths7Days += participant.Deaths;
                totalAssists7Days += participant.Assists;

                if(match.MatchInfo.GameStartTimestamp > startTimeUnix24Hours)
                {
                    games24Hours++;

                    if (participant.Win)
                    {
                        wins24Hours++;
                    }

                    totalKills24Hours += participant.Kills;
                    totalDeaths24Hours += participant.Deaths;
                    totalAssists24Hours += participant.Assists;
                }
            }

            var winrate7Days = (float)100 * wins7Days / games7Days;
            Console.WriteLine();
            Console.WriteLine($"Winrate for the past 7 days: {winrate7Days}%");

            var kda7Days = (float)(totalKills7Days + totalAssists7Days)/ totalDeaths7Days;

            Console.WriteLine();
            Console.WriteLine($"KDA for the past 7 days: {kda7Days}");

            var winrate24Hours = (float)100 * wins24Hours / games24Hours;
            Console.WriteLine();
            Console.WriteLine($"Winrate for the past 24 hours: {winrate24Hours}%");

            var kda24Hours = (float)(totalKills24Hours + totalAssists24Hours) / totalDeaths24Hours;

            Console.WriteLine();
            Console.WriteLine($"KDA for the past 24 hours: {kda24Hours}");
        }
    }
}
