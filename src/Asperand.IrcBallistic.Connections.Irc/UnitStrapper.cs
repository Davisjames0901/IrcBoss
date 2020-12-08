using Asperand.IrcBallistic.Connections.Irc.Modules;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.InversionOfControl.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.Connections.Irc
{
    public class UnitStrapper : BaseUnitStrapper<IrcConfiguration>
    {
        protected override IServiceCollection RegisterInternalDependencies(IServiceCollection services, IrcConfiguration config)
        {
            services.AddTransient<IConnection, IrcConnection>();
            services.AddTransient<IModule, PingModule>();
            services.AddSingleton(config);
            
            return services;
        }

        public override string Namespace => GetType().Namespace;
    }
}