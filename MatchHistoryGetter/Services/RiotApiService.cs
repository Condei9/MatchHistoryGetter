using MatchHistoryGetter.Models;
using Newtonsoft.Json;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using System.Linq;

namespace MatchHistoryGetter.Services
{
    public class RiotApiService : IRiotApiService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "RGAPI-4f6a4c3b-02bf-432f-85a8-0c9719add5e5";
        private const string MatchBaseUrl = "https://europe.api.riotgames.com/lol/match/v5/matches";
        private const string SummonerBaseUrl = "https://europe.api.riotgames.com/riot/account/v1/accounts/by-riot-id";

        public RiotApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<MatchModel>> GetMatchHistoryAsync(string summonerName, string tagLine)
        {
            _httpClient.DefaultRequestHeaders.Add("X-Riot-Token", ApiKey);

            string summonerPuuId = GetSummonerPuuId(summonerName, tagLine).Result;

            var matchHistoryResponse = await _httpClient.GetStringAsync($"{MatchBaseUrl}/by-puuid/{summonerPuuId}/ids?start=0&count=20");
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
            var baseAddress = new Uri($"{SummonerBaseUrl}/{summonerName}/{tagLine}");
            var summonerResponse = await _httpClient.GetStringAsync(baseAddress);
            var summoner = JsonConvert.DeserializeObject<dynamic>(summonerResponse);

            return summoner.puuid;
        }
    }
}
