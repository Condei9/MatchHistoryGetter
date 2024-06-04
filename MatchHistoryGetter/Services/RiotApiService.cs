using MatchHistoryGetter.Models;
using Newtonsoft.Json;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using System.Linq;
using System.Web;
using System.Text;

namespace MatchHistoryGetter.Services
{
    public class RiotApiService : IRiotApiService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "RGAPI-2c94524e-8fea-4d14-a93d-35927c8d9f85";
        private const string MatchBaseUrl = "https://europe.api.riotgames.com/lol/match/v5/matches";
        private const string SummonerBaseUrl = "https://europe.api.riotgames.com/riot/account/v1/accounts/by-riot-id";

        public RiotApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("X-Riot-Token", ApiKey);
        }

        public async Task<List<MatchModel>> GetMatchHistoryAsync(string summonerName, string tagLine)
        {
            var endTime = DateTime.UtcNow;
            var startTime7Days = endTime.AddDays(-7);

            long startTimeUnix = new DateTimeOffset(startTime7Days).ToUnixTimeSeconds();
            long endTimeUnix = new DateTimeOffset(endTime).ToUnixTimeSeconds();

            string summonerPuuId = GetSummonerPuuId(summonerName, tagLine).Result;

            var matchHistoryResponse = await _httpClient.GetStringAsync($"{MatchBaseUrl}/by-puuid/{summonerPuuId}/ids?startTime={startTimeUnix}&endTime={endTimeUnix}&count=100");
            var matchHistory = JsonConvert.DeserializeObject<dynamic>(matchHistoryResponse);

            var matches = new List<MatchModel>();
            foreach (var matchId in matchHistory)
            {
                var matchDataResponse = await _httpClient.GetStringAsync($"{MatchBaseUrl}/{matchId}?api_key={ApiKey}");
                var matchData = JsonConvert.DeserializeObject<dynamic>(matchDataResponse);


                var tempMatch = new MatchModel();

                tempMatch.MetadataInfo.DataVersion = matchData.metadata.dataVersion;
                tempMatch.MetadataInfo.MatchId = matchData.metadata.matchId;

                tempMatch.MatchInfo.EndOfGameResult = matchData.info.endOfGameResult;
                tempMatch.MatchInfo.GameCreation = matchData.info.gameCreation;
                tempMatch.MatchInfo.GameDuration = matchData.info.gameDuration;
                tempMatch.MatchInfo.GameId = matchData.info.gameId;
                tempMatch.MatchInfo.GameMode = matchData.info.gameMode;
                tempMatch.MatchInfo.GameName = matchData.info.gameName;
                tempMatch.MatchInfo.GameStartTimestamp = matchData.info.gameStartTimestamp;
                tempMatch.MatchInfo.GameType = matchData.info.gameType;
                tempMatch.MatchInfo.GameVersion = matchData.info.gameVersion;
                tempMatch.MatchInfo.MapId = matchData.info.mapId;
                tempMatch.MatchInfo.PlatformId = matchData.info.platformId;
                tempMatch.MatchInfo.QueueId = matchData.info.queueId;
                tempMatch.MatchInfo.TournamentCode = matchData.info.tournamentCode;

                tempMatch.MatchInfo.Participants.AddRange(matchData.info.participants.ToObject<List<ParticipantModel>>());

                matches.Add(tempMatch);

            }
            return matches;
        }

        public async Task<string> GetSummonerPuuId(string summonerName, string tagLine)
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(summonerName);
            
            var encodedName = HttpUtility.UrlEncode(utf8Bytes);
            var baseAddress = $"{SummonerBaseUrl}/{encodedName}/{tagLine}";
            var summonerResponse = await _httpClient.GetStringAsync(baseAddress);
            var summoner = JsonConvert.DeserializeObject<dynamic>(summonerResponse);

            return summoner.puuid;
        }
    }
}
