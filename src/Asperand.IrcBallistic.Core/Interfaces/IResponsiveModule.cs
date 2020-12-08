using System.Threading.Tasks;

namespace Asperand.IrcBallistic.Core.Interfaces
{
    public interface IResponsiveModule: IModule
    {
        public Task WriteMessage(IResponse response, IConnection source);
    }
}