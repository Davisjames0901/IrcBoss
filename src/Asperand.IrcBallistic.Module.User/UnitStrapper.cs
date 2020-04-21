using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Module.User.Data;
using AsperandLabs.UnitStrap.Core.Abstracts;
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