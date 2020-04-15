using Asperand.IrcBallistic.Connections.Irc;
using Asperand.IrcBallistic.Connections.Irc.Extensions;
using Asperand.IrcBallistic.Core.Extensions;
using Asperand.IrcBallistic.Module.Command.Extensions;
using Asperand.IrcBallistic.Module.User.Extensions;
using Asperand.IrcBallistic.Worker.Configuration;
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
            config.AddJsonFile("appsettings.private.json", true);
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
            var ircConfig = new IrcConfiguration();
            var youtubeConfig = new YoutubeConfig(); 
            hostContext.Configuration.GetSection("Youtube").Bind(youtubeConfig);
            hostContext.Configuration.GetSection("Connections:Irc").Bind(ircConfig);
            services.AddSingleton(youtubeConfig);
            
            services.AddHostedService<Worker>();
            services.AddCommandModule();
            services.AddUserModule();
            services.AddIrcConnection(ircConfig);
            services.AddIrcBallistic();
        }
    }
}