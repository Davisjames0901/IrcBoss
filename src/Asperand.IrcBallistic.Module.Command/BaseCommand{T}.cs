using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Core;
using Asperand.IrcBallistic.Module.Command.Attributes;
using Asperand.IrcBallistic.Module.Command.Data;
using Asperand.IrcBallistic.Module.Command.Enum;
using Asperand.IrcBallistic.Module.Command.Interfaces;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.Module.Command
{
    public abstract class BaseCommand<T> : ICommand where T : new()

    {
    public CommandExecutionContext Context { get; set; }

    public abstract Task<CommandResult> Execute(T options, CancellationToken token);

    public Task<CommandResult> Execute(string[] args, CancellationToken token)
    {
        var result = Parser.Default.ParseArguments<T>(args);
        if (!result.Errors.Any())
        {
            SetContent(args, result.Value);
            return Execute(result.Value, token);
        }

        return Task.FromResult(CommandResult.InvalidParams);
    }

    private void SetContent<T>(string[] content, T obj)
    {
        var contentProperties = typeof(T).GetProperties()
            .Where(x=> x.GetCustomAttributes(typeof(ContentAttribute), false).Length > 0);
        var contentValue = string.Join(' ', content);
        foreach (var property in contentProperties)
        {
            property.SetValue(obj, contentValue);
        }
    }

    public string GetHelpText()
    {
        return "";
    }

    protected Task SendMessage(string message, bool isAction = false)
    {
        return Context.SourceConnection.WriteMessage(new Response
        {
            Target = Context.Request.Target,
            Text = message,
            IsAction = isAction
        });
    }
    }
}