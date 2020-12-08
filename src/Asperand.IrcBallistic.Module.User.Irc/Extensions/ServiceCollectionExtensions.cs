using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.Module.User.Irc.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIrcUserModule(this IServiceCollection services, bool addAnalyzer = false)
        {
            return new UnitStrapper().RegisterDependencies(services, addAnalyzer);
        }
    }
}