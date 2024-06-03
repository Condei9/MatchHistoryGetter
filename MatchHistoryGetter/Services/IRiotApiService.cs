﻿using MatchHistoryGetter.Models;

namespace MatchHistoryGetter.Services
{
    public interface IRiotApiService
    {
        Task<List<MatchModel>> GetMatchHistoryAsync(string summonerName, string tagLine);
    }
}
