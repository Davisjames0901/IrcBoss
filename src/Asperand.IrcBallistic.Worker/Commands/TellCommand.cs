using System;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Core.Events;
using Asperand.IrcBallistic.Module.Command;
using Asperand.IrcBallistic.Module.Command.Attributes;
using Asperand.IrcBallistic.Module.Command.Enum;

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
        public override async Task<CommandResult> Execute(CancellationToken token)
        {
            await SendMessage("You can count on me! (for 24 hours at least :D)");
            
            var startTime = DateTime.Now;
            _isActive = true;
            RegisterMessageCallback(Tell);
            while (_isActive && DateTime.Now < startTime.AddDays(1))
            {
                await Task.Delay(1000, token);
            }
            
            return _isActive ? CommandResult.Failed : CommandResult.Success;
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