using Asperand.IrcBallistic.Core.Extensions;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Module.Command.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.Module.Command.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandModule(this IServiceCollection services, bool addAnalyzer = false)
        {
            return new UnitStrapper().RegisterDependencies(services, addAnalyzer);
        }
    }
}