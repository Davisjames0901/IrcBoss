using System.Threading.Tasks;
using Asperand.IrcBallistic.InversionOfControl;
using Microsoft.Extensions.Hosting;

namespace Asperand.IrcBallistic.Worker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args, new Bootstrapper()).RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, Bootstrapper bootstrapper) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) => bootstrapper.ConfigureConfiguration(hostingContext, config, args))
                .ConfigureServices(bootstrapper.ConfigureServices)
                .ConfigureLogging(bootstrapper.ConfigureLogging);
    }
}