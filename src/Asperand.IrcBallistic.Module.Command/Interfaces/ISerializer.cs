using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Module.Command.Data;

namespace Asperand.IrcBallistic.Module.Command.Interfaces
{
    public interface ISerializer
    {
        CommandRequest Deserialize(IRequest request);
        string Serialize(IResponse result);
    }
}