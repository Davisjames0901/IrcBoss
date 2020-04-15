using Asperand.IrcBallistic.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.Connections.Irc.Extensions
{
    public static class ServiceCollectionExtensions
    {
        
        public static IServiceCollection AddIrcConnection(this IServiceCollection services, IrcConfiguration config)
        {
            services.AddTransient<IConnection, IrcConnection>();
            services.AddTransient<IrcSerializer>();
            services.AddSingleton(config);

            return services;
        }
    }
}