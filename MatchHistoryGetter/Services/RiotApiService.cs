using MatchHistoryGetter.Models;

namespace MatchHistoryGetter.Services
{
    public class RiotApiService : IRiotApiService
    {
        private readonly HttpClient _httpClient;

        public RiotApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<List<MatchModel>> GetMatchHistoryAsync(string summonerName, string tagLine)
        {
            return null;
        }
    }
}
