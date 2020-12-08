using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.Module.Command.Irc.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIrcCommandModule(this IServiceCollection services, CommandConfig config, bool addAnalyzer = false)
        {
            return new UnitStrapper().RegisterDependencies(services, config, addAnalyzer);
        }
    }
}