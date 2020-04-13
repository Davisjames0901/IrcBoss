using System;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Attributes;
using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Messages;

namespace Asperand.IrcBallistic.Worker.Commands
{
    [CommandGroup("tell")]
    public class TellCommand : BaseCommand
    {
        private bool _isActive;
        private DateTime _startTime;
        public async override Task<CommandExecutionResult> Execute(CommandRequest request, CancellationToken token)
        {
            if (!request.Flags.ContainsKey("u") && !request.Flags.ContainsKey("m"))
            {
                await Connection.SendMessage(new MessageResponse
                {
                    Target = request.Target,
                    Text = "use -u for the username and -m for the message."
                });
                return CommandExecutionResult.Failed;
            }
            await Connection.SendMessage(new MessageResponse
            {
                Target = request.Target,
                Text = "You can count on me! (for 24 hours at least :D)"
            });
            _startTime = DateTime.Now;
            _isActive = true;
            var id = Connection.RegisterCallback(EventType.Message, e =>
            {
                if(string.Equals(request.Flags["u"], (e as MessageRequest)?.SourceUserName, StringComparison.OrdinalIgnoreCase))
                {
                    Connection.SendMessage(new MessageResponse
                    {
                        Target = request.Target,
                        Text = $"{request.Flags["u"]}, {request.RequesterUsername} wanted me to tell you '{request.Flags["m"]}'"
                    });
                    _isActive = false;
                }
            });
            while (_isActive)
            {
                if (DateTime.Now > _startTime.AddDays(1))
                {
                    await Connection.SendMessage(new MessageResponse
                    {
                        Target = request.Target,
                        Text = $"feeling faint... must.. relay.. message.. X("
                    });
                    Connection.RemoveCallback(id);
                    return CommandExecutionResult.Failed;
                }
                await Task.Delay(1000);
            }
            
            Connection.RemoveCallback(id);
            return CommandExecutionResult.Success;
        }
    }
}