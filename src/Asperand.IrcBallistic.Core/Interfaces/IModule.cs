namespace Asperand.IrcBallistic.Core.Interfaces
{
    public interface IModule
    {
        bool IsEagerModule { get; }
        bool IsReinstatable { get; }
        void RegisterConnection(IConnection connection);
    }
}