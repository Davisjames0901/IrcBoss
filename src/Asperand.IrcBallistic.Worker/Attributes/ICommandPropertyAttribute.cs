using System;
using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Messages;

namespace Asperand.IrcBallistic.Worker.Attributes
{
  public interface ICommandPropertyAttribute
  {
    CommandValidationResult Validate(Type callerType, Request message);
  }
}