using Asperand.IrcBallistic.Worker.Interfaces;

namespace Asperand.IrcBallistic.Worker.Modules
{
    public interface IModule
    {
        bool IsEagerModule { get; }
        bool IsReinstatable { get; }
        void RegisterConnection(IConnection connection);
    }
}