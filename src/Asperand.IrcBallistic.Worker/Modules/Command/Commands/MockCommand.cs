using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Attributes;
using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Messages;

namespace Asperand.IrcBallistic.Worker.Commands
{
    [CommandGroup("mock", "Mocks the target")]
    public class MockCommand : BaseCommand
    {
        private readonly UserContainer _users;
        public MockCommand(UserContainer users)
        {
            _users = users;
        }
        
        public override async Task<CommandResult> Execute(CommandRequest request, CancellationToken token)
        {
            var target = request.Flags.ContainsKey("u") ? request.Flags["u"] : request.Content;
            var lastMessage = _users.GetLastMessageByName(target);
            if (string.IsNullOrWhiteSpace(lastMessage))
                return CommandResult.Failed;

            await SendMessage($"<{target}> "+new string(lastMessage.Select((x, i) => i % 2 != 0 ? char.ToUpper(x) : char.ToLower(x)).ToArray()));
            return CommandResult.Success;
        }
    }
}