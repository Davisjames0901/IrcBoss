using System;

namespace Asperand.IrcBallistic.InversionOfControl.Abstracts
{
    public abstract class DependencyAnalyzer
    {
        internal readonly UnitStrapperContainer _scopedContainer;
        public abstract bool IsConfigured { get; }
        public abstract string Namespace { get; }

        public DependencyAnalyzer()
        {
            _scopedContainer = new UnitStrapperContainer(Namespace);
        }

        public RegistrationAnalysis GetAnalysis(IServiceProvider provider)
        {
            return _scopedContainer.AnalyzeRegistrations(provider);
        }
    }
}