using System;
using System.Threading.Tasks;
using MatchHistoryGetter.Controllers;
using MatchHistoryGetter;
using Microsoft.Extensions.DependencyInjection;

namespace MatchHistoryGetter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            var startup = new Startup();
            startup.ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var matchHistoryController = serviceProvider.GetService<MatchHistoryController>();

            Console.WriteLine("Name:");
            var summonerName = Console.ReadLine();

            Console.WriteLine("Tagline:");
            var tagLine = Console.ReadLine();

            if (matchHistoryController != null)
            {
                await matchHistoryController.GetMatchHistory(summonerName, tagLine);
            }
        }
    }
}
