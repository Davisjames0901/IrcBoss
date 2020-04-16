using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.InversionOfControl.Extenstions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUnitStrapper(this IServiceCollection services)
        {
            services.AddSingleton(services);
            services.AddTransient<ContainerValidator>();
            
            return services;
        }
    }
}