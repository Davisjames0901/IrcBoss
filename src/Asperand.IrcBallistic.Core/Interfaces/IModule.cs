using System.Threading.Tasks;

namespace Asperand.IrcBallistic.Core.Interfaces
{
    public interface IModule
    {
        bool IsEagerModule { get; }
        public Task Handle<T>(IRequest payload, T connection) where T : IConnection;
    }
}