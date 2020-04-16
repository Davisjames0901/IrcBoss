using System;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.InversionOfControl;
using Asperand.IrcBallistic.InversionOfControl.Abstracts;
using Asperand.IrcBallistic.Module.User.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.Module.User
{
    public class UnitStrapper : BaseUnitStrapper
    {
        protected override IServiceCollection RegisterInternalDependencies(IServiceCollection services)
        {
            services.AddSingleton(new UserContainer());
            services.AddTransient<IModule, UserModule>();

            return services;
        }
        
        public override string Namespace => GetType().Namespace;
    }
}