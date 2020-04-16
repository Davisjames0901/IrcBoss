using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.InversionOfControl.Abstracts
{
    public abstract class BaseUnitStrapper : DependencyAnalyzer
    {
        private bool _isConfigured;
        public override bool IsConfigured => _isConfigured;

        public IServiceCollection RegisterDependencies(IServiceCollection services, bool registerAnalyzer)
        {
            if (registerAnalyzer)
            {
                _isConfigured = true;
                services.AddSingleton<DependencyAnalyzer>(this);
                RegisterInternalDependencies(_scopedContainer);
            }
            return RegisterInternalDependencies(services);
        }
        
        protected abstract IServiceCollection RegisterInternalDependencies(IServiceCollection services);
    }
}