using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public static IEnumerable<Type> MapImplementationTypes(this IServiceCollection services, IEnumerable<Type> types)
        {
            return types.Select(t =>
            {
                var service = services.First(s => s.ServiceType == t);
                if (service.ImplementationType == null)
                    return t;
                return service.ImplementationType;
            });
        }
    }
}