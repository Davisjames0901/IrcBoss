using Asperand.IrcBallistic.Core.Events;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Module.User.Data;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Module.User
{
    public class UserModule : IModule
    {
        private readonly UserContainer _userContainer;
        private readonly ILogger<UserModule> _log;
        public UserModule(UserContainer userContainer, ILogger<UserModule> log)
        {
            _userContainer = userContainer;
            _log = log;
        }
        
        public bool IsEagerModule => true;
        public bool IsReinstatable => false;
        public void RegisterConnection(IConnection connection)
        {
            connection.RegisterCallback(EventType.UserDiscovery, e => HandleDiscovery(e as UserDiscovery));
            connection.RegisterCallback(EventType.UserEvent, e => HandleUserEvent(e as UserEvent));
            connection.RegisterCallback(EventType.Message, e => RecordLastMessage(e as MessageRequest));
        }

        private void HandleDiscovery(UserDiscovery discovery)
        {
            _log.LogInformation("Received user discovery!");
            foreach (var user in discovery.Usernames)
            {
                _log.LogInformation($"Adding User {user}");
                _userContainer.Users.TryAdd(user.ToLower(), new Data.User
                {
                    Name = user
                });
            }
        }

        private void RecordLastMessage(MessageRequest request)
        {
            _log.LogInformation($"Updating user: {request.SourceUserName}'s last message");
            var user = _userContainer.GetUserByName(request.SourceUserName);
            if (user == null)
            {
                _log.LogError("Attempted to update a user that didnt exit.");
                return;
            }

            user.LastMessage = request.Text;
        }

        private void HandleUserEvent(UserEvent e)
        {
            if (e.Event == UserEventEnum.Joined)
            {
                _userContainer.Users.TryAdd(e.Username.ToLower(), new Data.User
                {
                    Name = e.Username
                });
                _log.LogInformation($"User joined, {e.Username}");
                return;
            }
            _log.LogInformation($"User left, {e.Username}");
            _userContainer.Users.TryRemove(e.Username.ToLower(), out _);
        }
    }
}