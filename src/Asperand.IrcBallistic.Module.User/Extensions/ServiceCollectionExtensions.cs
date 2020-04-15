using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Module.User.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.Module.User.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserModule(this IServiceCollection services)
        {
            services.AddSingleton(new UserContainer());
            services.AddTransient<IModule, UserModule>();
            
            
            return services;
        }
    }
}