using System;
using Asperand.IrcBallistic.Core.Extensions;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.InversionOfControl;
using Asperand.IrcBallistic.InversionOfControl.Abstracts;
using Asperand.IrcBallistic.Module.Command.Engine;
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