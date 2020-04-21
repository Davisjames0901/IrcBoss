using Asperand.IrcBallistic.Core.Extensions;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Module.Command.Engine;
using AsperandLabs.UnitStrap.Core.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.Module.Command
{
    public class UnitStrapper : BaseUnitStrapper
    {
        protected override IServiceCollection RegisterInternalDependencies(IServiceCollection services)
        {
            services.AddSingleton<CommandEngine>();
            services.AddSingleton<CommandMetadataAccessor>();
            services.AddTransient<ArgumentParser>();
            services.AddTransient<IModule, CommandModule>();
            services.AddAllInheritorsTransient<ICommand>();

            return services;
        }
        
        public override string Namespace => GetType().Namespace;
    }
}