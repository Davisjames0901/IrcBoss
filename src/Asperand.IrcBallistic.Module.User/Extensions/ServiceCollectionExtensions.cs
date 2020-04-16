using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Module.User.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.Module.User.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserModule(this IServiceCollection services, bool addAnalyzer = false)
        {
            return new UnitStrapper().RegisterDependencies(services, addAnalyzer);
        }
    }
}