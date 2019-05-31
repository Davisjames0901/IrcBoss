using System;
using Digman.Io.IrcBalistic.Classes;

namespace Digman.Io.IrcBalistic.Attributes
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

    public CommandValidationResult Validate(Type callerType, Message message)
    {
      return new CommandValidationResult{
        IsValid = true
      };
    }
  }
}