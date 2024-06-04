using MatchHistoryGetter.Models;
using MatchHistoryGetter.Services;
using System.Text.RegularExpressions;

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

            if (string.IsNullOrEmpty(puuid))
            {
                return;
            }

            var startTime24Hours = DateTime.UtcNow.AddDays(-1);
            long startTimeUnix24Hours = new DateTimeOffset(startTime24Hours).ToUnixTimeMilliseconds();

            var stats7Days = CalculateStats(matches, puuid, null);
            var stats24Hours = CalculateStats(matches, puuid, startTimeUnix24Hours);

            PrintMatchDetails(matches, puuid);
            PrintStatistics("7 days", stats7Days);
            PrintStatistics("24 hours", stats24Hours);
        }

        private StatsModel CalculateStats(IEnumerable<MatchModel> matches, string puuid, long? startTimeUnix)
        {
            var wins = 0;
            var games = 0;
            var totalKills = 0;
            var totalDeaths = 0;
            var totalAssists = 0;

            foreach (var match in matches)
            {
                if (startTimeUnix.HasValue && match.MatchInfo.GameStartTimestamp <= startTimeUnix.Value)
                {
                    continue;
                }

                games++;
                var participant = match.MatchInfo.Participants.FirstOrDefault(x => x.Puuid == puuid);

                if (participant.Win)
                {
                    wins++;
                }

                totalKills += participant.Kills;
                totalDeaths += participant.Deaths;
                totalAssists += participant.Assists;
            }

            return new StatsModel
            {
                Wins = wins,
                Games = games,
                TotalKills = totalKills,
                TotalDeaths = totalDeaths,
                TotalAssists = totalAssists
            };
        }

        private void PrintMatchDetails(IEnumerable<MatchModel> matches, string puuid)
        {
            Console.WriteLine("\nChampionName    Kills/Deaths/Assists      Result");
            Console.WriteLine("----------------------------------------------------");

            foreach (var match in matches)
            {
                var participant = match.MatchInfo.Participants.FirstOrDefault(x => x.Puuid == puuid);
                var matchWon = participant.Win ? "Win" : "Loss";

                Console.WriteLine($"{participant.ChampionName}           {participant.Kills}/{participant.Deaths}/{participant.Assists}                 Result: {matchWon}");
            }
        }

        private void PrintStatistics(string period, StatsModel stats)
        {
            var winRate = stats.Games > 0 ? (float)100 * stats.Wins / stats.Games : 0;
            var kda = stats.TotalDeaths > 0 ? (float)(stats.TotalKills + stats.TotalAssists) / stats.TotalDeaths : 0;

            Console.WriteLine();
            Console.WriteLine($"Winrate for the past {period}: {winRate}%");
            Console.WriteLine($"KDA for the past {period}: {kda}\n");
        }
    }
}
