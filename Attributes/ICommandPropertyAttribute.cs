using System;
using Digman.Io.IrcBalistic.Classes;

namespace Digman.Io.IrcBalistic.Attributes
{
  public interface ICommandPropertyAttribute
  {
    CommandValidationResult Validate(Type callerType, Message message);
  }
}