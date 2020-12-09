using System;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Core.Module;

namespace Asperand.IrcBallistic.Core.Interfaces
{
    public interface IModule
    {
        bool IsEagerModule { get; }
        int TimeoutSeconds { get; }
        ModuleStatistics ModuleStatistics { get; }
        Task Handle<T>(IRequest payload, T connection) where T : IConnection;
        void RegisterTroubleCallback(Action<ModuleStatistics> action);
    }
}