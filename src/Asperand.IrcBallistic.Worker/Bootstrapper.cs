using Asperand.IrcBallistic.Worker.Configuration;
using Asperand.IrcBallistic.Worker.Connections;
using Asperand.IrcBallistic.Worker.Extensions;
using Asperand.IrcBallistic.Worker.Modules;
using Asperand.IrcBallistic.Worker.Modules.Command;
using Asperand.IrcBallistic.Worker.Modules.Command.Dependencies;
using Asperand.IrcBallistic.Worker.Modules.UserManagement;
using Asperand.IrcBallistic.Worker.Modules.UserManagement.Dependencies;
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
            services.AddHostedService<Worker>();
            services.AddSingleton(services);
            services.AddSingleton(youtubeConfig);
            services.AddSingleton(ircConfig);
            services.AddSingleton(new UserContainer());
            services.AddSingleton<ConnectionManager>();
            services.AddSingleton<CommandEngine>();
            
            services.AddTransient<IConnection, IrcConnection>();
            services.AddTransient<IrcSerializer>();
            services.AddTransient<CommandMetadataAccessor>();
            services.AddTransient<ArgumentParser>();
            services.AddTransient<IModule, CommandModule>();
            services.AddTransient<IModule, UserManagementModule>();
            services.AddAllInheritorsTransient<ICommand>();
        }
    }
}