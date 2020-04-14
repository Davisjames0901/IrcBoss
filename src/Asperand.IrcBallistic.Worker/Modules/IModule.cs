using Asperand.IrcBallistic.Worker.Connections;

namespace Asperand.IrcBallistic.Worker.Modules
{
    public interface IModule
    {
        bool IsEagerModule { get; }
        bool IsReinstatable { get; }
        void RegisterConnection(IConnection connection);
    }
}