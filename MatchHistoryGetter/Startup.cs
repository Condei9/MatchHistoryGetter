using MatchHistoryGetter.Services;

namespace MatchHistoryGetter
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<IRiotApiService, RiotApiService>();
        }
    }
}
