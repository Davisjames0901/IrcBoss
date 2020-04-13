using System;
using System.Linq;
using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Interfaces;
using Asperand.IrcBallistic.Worker.Messages;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Worker.Modules.UserManagment
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
            connection.RegisterCallback(EventType.Message, e => RecordLastMessage(e as MessageRequest));
        }

        private void HandleDiscovery(UserDiscovery discovery)
        {
            _log.LogInformation("Received user discovery!");
            foreach (var user in discovery.Usernames)
            {
                _log.LogInformation($"Adding User {user}");
                _userContainer.Users.Add(new User
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
    }
}