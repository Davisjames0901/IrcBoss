using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Attributes;
using Asperand.IrcBallistic.Worker.Classes;

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

        [Content]
        public string Content { get; set; }
        
        public override async Task<CommandResult> Execute(CancellationToken token)
        {
            var lastMessage = _users.GetLastMessageByName(Content);
            if (string.IsNullOrWhiteSpace(lastMessage))
                return CommandResult.Failed;

            await SendMessage($"<{Content}> "+new string(lastMessage.Select((x, i) => i % 2 != 0 ? char.ToUpper(x) : char.ToLower(x)).ToArray()));
            return CommandResult.Success;
        }
    }
}