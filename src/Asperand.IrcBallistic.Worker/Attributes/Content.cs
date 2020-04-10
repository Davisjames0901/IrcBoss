using System;
using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Messages;

namespace Asperand.IrcBallistic.Worker.Attributes
{
  [AttributeUsage(AttributeTargets.Property)]
  public class Content: Attribute, ICommandPropertyAttribute
  {
    private readonly bool _includeCommand;

    public Content(bool includeCommand = false)
    {
        _includeCommand = includeCommand;
    }
    public bool IncludeCommand => _includeCommand;

    public CommandValidationResult Validate(Type callerType, Request message)
    {
      return new CommandValidationResult{
        IsValid = true
      };
    }
  }
}