using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Commands;
using Asperand.IrcBallistic.Worker.Configuration;
using Asperand.IrcBallistic.Worker.Connections;
using Asperand.IrcBallistic.Worker.Extensions;
using Asperand.IrcBallistic.Worker.Interfaces;
using Asperand.IrcBallistic.Worker.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Worker
{
    public static class Bootstrapper
    {
        public static void ConfigureConfiguration(HostBuilderContext hostContext, IConfigurationBuilder config, string[] args)
        {
            config.AddJsonFile("appsettings.json", true);
            config.AddEnvironmentVariables();

            if (args != null)
            {
                config.AddCommandLine(args);
            }
        }
        
        public static void ConfigureLogging(HostBuilderContext hostContext, ILoggingBuilder logging)
        {
            logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
            logging.AddConsole();
            
        }
        
        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {

            var config = new IrcConfiguration
            {
                Channel = "#davc",
                DefaultNickname = "DrDigBotTesting",
                MessageFlag = '!',
                ServerHostName = "irc.freenode.net",
                ServerPort = 6667
            };
            services.AddHostedService<Worker>();
            services.AddSingleton(services);
            services.AddSingleton(config);
            services.AddSingleton(new UserContainer());
            services.AddSingleton<ConnectionManager>();
            
            services.AddTransient<CommandEngine>();
            services.AddTransient<IConnection, IrcConnection>();
            services.AddTransient<IrcSerializer>();
            services.AddTransient<CommandLocator>();
            services.AddAllInheritorsTransient<ICommand>();
        }
    }
}