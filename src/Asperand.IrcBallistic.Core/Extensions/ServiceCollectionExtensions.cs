using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAllInheritorsTransient<T>(this IServiceCollection services)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(T).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);

            foreach (var type in types)
            {
                services.AddTransient(typeof(T), type);
                services.AddTransient(type);
            }

            return services;
        }
        
        public static IServiceCollection AddIrcBallistic(this IServiceCollection services)
        {
            services.AddSingleton<ConnectionManager>();

            return services;
        }
    }
}