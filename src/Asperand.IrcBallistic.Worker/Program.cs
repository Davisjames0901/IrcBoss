using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Asperand.IrcBallistic.Worker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) => Bootstrapper.ConfigureConfiguration(hostingContext, config, args))
                .ConfigureServices(Bootstrapper.ConfigureServices)
                .ConfigureLogging(Bootstrapper.ConfigureLogging);
    }
}