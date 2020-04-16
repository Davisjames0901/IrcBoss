using System;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.InversionOfControl;
using Asperand.IrcBallistic.InversionOfControl.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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