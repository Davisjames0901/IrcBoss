using System;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Attributes;
using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Messages;

namespace Asperand.IrcBallistic.Worker.Commands
{
    [CommandGroup("tell", "Waits for a user to be active then delivers a message.")]
    public class TellCommand : BaseCommand
    {
        [Flag("u", "The target user")]
        public string Username { get; set; }
        
        [Flag("m", "The message to deliver")]
        public string Message { get; set;  }

        private bool _isActive;
        public override async Task<CommandExecutionResult> Execute(CommandRequest request, CancellationToken token)
        {
            await SendMessage("You can count on me! (for 24 hours at least :D)");
            
            var startTime = DateTime.Now;
            _isActive = true;
            RegisterMessageCallback(Tell);
            while (_isActive)
            {
                if (DateTime.Now > startTime.AddDays(1) && !token.IsCancellationRequested)
                {
                    await SendMessage($"feeling faint... must.. relay.. message.. DX");
                    return CommandExecutionResult.Failed;
                }
                await Task.Delay(10000, token);
            }
            
            return CommandExecutionResult.Success;
        }

        private void Tell(MessageRequest e)
        {
            if(string.Equals(Username, e.SourceUserName, StringComparison.OrdinalIgnoreCase))
            {
                SendMessage($"{Username}, {Context.Request.RequesterUsername} wanted me to tell you '{Message}'");
                _isActive = false;
            }
        }
    }
}