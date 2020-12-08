using Asperand.IrcBallistic.Core.Extensions;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.InversionOfControl.Abstracts;
using Asperand.IrcBallistic.Module.Command.Engine;
using Asperand.IrcBallistic.Module.Command.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.Module.Command.Irc
{
    public class UnitStrapper : BaseUnitStrapper<CommandConfig>
    {
        protected override IServiceCollection RegisterInternalDependencies(IServiceCollection services, CommandConfig config)
        {
            services.AddSingleton(config);
            services.AddSingleton<CommandEngine>();
            services.AddSingleton<CommandMetadataAccessor>();
            services.AddTransient<ArgumentParser>();
            services.AddTransient<IModule, CommandModule>();
            services.AddTransient<ISerializer, IrcSerializer>();
            services.AddAllInheritorsTransient<ICommand>();

            return services;
        }
        
        public override string Namespace => GetType().Namespace;
    }
}