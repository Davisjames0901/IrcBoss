using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Attributes;
using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Messages;

namespace Asperand.IrcBallistic.Worker.Commands
{
  [CommandGroup("echo")]
  [HelpText("Echos the input provided.")]
  public class Echo : BaseCommand
  {
    public override async Task<CommandExecutionResult> Execute(CommandRequest request)
    {
      var response = new Response();
      response.Target = "#davc";
      if(request.Content.StartsWith("/me"))
      {
        response.Text = request.Content
          .Substring(3, request.Content.Length - 3)
          .Trim();
      }
      else 
      {
        response.Text = request.Content;
      }

      await Connection.SendMessage(response);
      return CommandExecutionResult.Success;
    }

    public override CommandRequest ValidateAndParse(Request request)
    {
      return new CommandRequest
      {
        Content = string.Join(' ', request.Text.Split(' ').Skip(1)),
        CommandName = "echo",
        Flags = null,
        Raw = request.Text,
        //Requester = request.SourceUserName
      };
    }
  }
}