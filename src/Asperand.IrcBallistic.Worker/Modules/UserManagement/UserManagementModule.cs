using System.Linq;
using Asperand.IrcBallistic.Worker.Connections;
using Asperand.IrcBallistic.Worker.Events;
using Asperand.IrcBallistic.Worker.Modules.UserManagement.Dependencies;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Worker.Modules.UserManagement
{
    public class UserManagementModule : IModule
    {
        private readonly UserContainer _userContainer;
        private readonly ILogger<UserManagementModule> _log;
        public UserManagementModule(UserContainer userContainer, ILogger<UserManagementModule> log)
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
                _userContainer.Users.TryAdd(user.ToLower(), new User
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
                _userContainer.Users.TryAdd(e.Username.ToLower(), new User
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