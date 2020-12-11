using System.Threading.Tasks;

namespace Asperand.IrcBallistic.Core.Interfaces
{
    public interface IModule
    {
        bool IsEagerModule { get; }
        int TimeoutSeconds { get; }
    }
}