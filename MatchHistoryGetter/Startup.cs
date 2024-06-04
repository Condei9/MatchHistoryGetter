using MatchHistoryGetter.Controllers;
using MatchHistoryGetter.Services;

namespace MatchHistoryGetter
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<IRiotApiService, RiotApiService>();
            services.AddTransient<MatchHistoryController>();

            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        }
    }
}
