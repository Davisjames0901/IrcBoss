using System.Threading.Tasks;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Core.Module;
using Asperand.IrcBallistic.Module.User.Data;
using Asperand.IrcBallistic.Module.User.Events;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Module.User
{
    public class UserModule : ModuleBase
    {
        private readonly ISerializer _serializer;
        private readonly UserContainer _userContainer;
        private readonly ILogger<UserModule> _log;
        public UserModule(ISerializer serializer, UserContainer userContainer, ILogger<UserModule> log) : base(log)
        {
            _serializer = serializer;
            _userContainer = userContainer;
            _log = log;
        }
        
        public override bool IsEagerModule => true;
        public override int TimeoutSeconds => 10;

        protected override Task<ModuleResult> Execute<T>(IRequest requestMessage, T connection)
        {
            var request = _serializer.Deserialize(requestMessage);
            var result = ModuleResult.Nop;
            switch (request)
            {
                case UserDiscovery userDiscovery:
                    HandleDiscovery(userDiscovery);
                    result = ModuleResult.Op;
                    break;
                case UserEvent userEvent:
                    HandleUserEvent(userEvent);
                    result = ModuleResult.Op;
                    break;
            }
            return Task.FromResult(result);
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

        //todo: re-add this functionality
        // private void RecordLastMessage(IRequest request)
        // {
        //     _log.LogInformation($"Updating user: {request.SourceUserName}'s last message");
        //     var user = _userContainer.GetUserByName(request.SourceUserName);
        //     if (user == null)
        //     {
        //         _log.LogError("Attempted to update a user that didnt exit.");
        //         return;
        //     }
        //
        //     user.LastMessage = request.Text;
        // }

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