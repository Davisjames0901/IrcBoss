using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Configuration;
using Asperand.IrcBallistic.Worker.Connections;
using Asperand.IrcBallistic.Worker.Interfaces;

using Asperand.IrcBallistic.Worker.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Asperand.IrcBallistic.Worker
{
    public static class Bootstrapper
    {
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
        }
    }
}