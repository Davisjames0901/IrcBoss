using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.InversionOfControl.Abstracts
{
    public abstract class BaseUnitStrapper<T> : DependencyAnalyzer
    {
        private bool _isConfigured;
        public override bool IsConfigured => _isConfigured;

        public IServiceCollection RegisterDependencies(IServiceCollection services, T config, bool registerAnalyzer = false)
        {
            if (registerAnalyzer)
            {
                _isConfigured = true;
                services.AddSingleton<DependencyAnalyzer>(this);
                RegisterInternalDependencies(_scopedContainer, config);
            }
            return RegisterInternalDependencies(services, config);
        }
        
        protected abstract IServiceCollection RegisterInternalDependencies(IServiceCollection services, T config);
    }
}