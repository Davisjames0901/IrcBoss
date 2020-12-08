using Asperand.IrcBallistic.Core.Interfaces;

namespace Asperand.IrcBallistic.Module.User
{
    public interface ISerializer
    {
        IEvent Deserialize(IRequest requestMessage);
    }
}