using Asperand.IrcBallistic.InversionOfControl.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.Core
{
    public class UnitStrapper : BaseUnitStrapper
    {
        protected override IServiceCollection RegisterInternalDependencies(IServiceCollection services)
        {
            services.AddSingleton<ConnectionManager>();

            return services;
        }
        
        public override string Namespace => GetType().Namespace;
    }
}