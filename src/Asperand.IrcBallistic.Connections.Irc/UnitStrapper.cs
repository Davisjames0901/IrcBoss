using Asperand.IrcBallistic.Core.Interfaces;
using AsperandLabs.UnitStrap.Core.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.Connections.Irc
{
    public class UnitStrapper : BaseUnitStrapper<IrcConfiguration>
    {
        protected override IServiceCollection RegisterInternalDependencies(IServiceCollection services, IrcConfiguration config)
        {
            services.AddTransient<IConnection, IrcConnection>();
            services.AddTransient<IrcSerializer>();
            services.AddSingleton(config);
            
            return services;
        }

        public override string Namespace => GetType().Namespace;
    }
}