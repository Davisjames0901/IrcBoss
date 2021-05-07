using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Module.Command;
using Asperand.IrcBallistic.Module.Command.Attributes;
using Asperand.IrcBallistic.Module.Command.Enum;
using Asperand.IrcBallistic.Module.User.Data;

namespace Asperand.IrcBallistic.Worker.Commands
{
    [CommandGroup("mock", "Mocks the target")]
    public class MockCommand : BaseCommand<MockOptions>
    {
        private readonly UserContainer _users;

        public MockCommand(UserContainer users)
        {
            _users = users;
        }

        public override async Task<CommandResult> Execute(MockOptions options, CancellationToken token)
        {
            var lastMessage = _users.GetLastMessageByName(options.User);
            if (string.IsNullOrWhiteSpace(lastMessage))
                return CommandResult.Failed;

            await SendMessage($"<{options.User}> " +
                              new string(lastMessage.Select((x, i) => i % 2 != 0 ? char.ToUpper(x) : char.ToLower(x))
                                  .ToArray()));
            return CommandResult.Success;
        }
    }

    public class MockOptions
    {
        public string User { get; set; }
    }
}