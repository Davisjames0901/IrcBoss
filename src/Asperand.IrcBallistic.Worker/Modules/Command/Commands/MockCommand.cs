using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Attributes;
using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Messages;

namespace Asperand.IrcBallistic.Worker.Commands
{
    [CommandGroup("mock")]
    [HelpText("Mocks the target")]
    public class MockCommand : BaseCommand
    {
        private readonly UserContainer _users;
        public MockCommand(UserContainer users)
        {
            _users = users;
        }
        
        public override async Task<CommandExecutionResult> Execute(CommandRequest request, CancellationToken token)
        {
            var target = request.Flags.ContainsKey("u") ? request.Flags["u"] : request.Content;
            var lastMessage = _users.GetLastMessageByName(target);
            if (string.IsNullOrWhiteSpace(lastMessage))
                return CommandExecutionResult.Failed;

            await Connection.SendMessage(new MessageResponse
            {
                Target = request.Target,
                Text = $"<{target}> "+new string(lastMessage.Select((x, i) => i % 2 != 0 ? char.ToUpper(x) : char.ToLower(x)).ToArray()),
            });
            return CommandExecutionResult.Success;
        }
    }
}