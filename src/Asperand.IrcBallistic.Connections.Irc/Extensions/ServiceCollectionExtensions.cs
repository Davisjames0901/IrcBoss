using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.Connections.Irc.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIrcConnection(this IServiceCollection services, IrcConfiguration config, bool addAnalyzer = false)
        {
            return new UnitStrapper().RegisterDependencies(services, config, addAnalyzer);
        }
    }
}