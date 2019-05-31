using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Digman.Io.IrcBalistic.Attributes;
using Digman.Io.IrcBalistic.Commands;

namespace Digman.Io.IrcBalistic.Classes
{
  public class CommandEngine
  {

    //Executes the command, Currently looks at the assembly to load the commands. Eventually I want to move to baking the commands at start up to speed things up a bit.
    public ResponsePacket ExecuteCommand(Message message)
    {
      //Get all commands in the assembly
      var type = typeof(ICommand);
      var types = AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(s => s.GetTypes())
          .Where(p => type.IsAssignableFrom(p));

      //Get the command group that is being requested, We want to throw an error if there are more than one with the same name.
      var commandType = types
          .SingleOrDefault(x => (Attribute.GetCustomAttribute(x, typeof(CommandGroup)) as CommandGroup)?.Value == message.CommandGroup.ToLower());
      if (commandType == null)
      {
        return null;
      }

      //Because the deserialzer doesnt know about the commands. it cant tell between args and sub commands so we will process them here now that our command group is known.
      message = RecompileCommandArgs(message, GetCommandNames(commandType));

      //creates and populates command group instance
      var commandGroup = CreateCommandInstance(commandType, message);

      //Build error response and return it if populated.
      var errorResponse = BuildErrorResponsePacket(commandGroup, message);
      if (errorResponse != null)
      {
        return errorResponse;
      }

      var response = ExecuteCommand(commandGroup.Command, commandType, message);

      response.Responses.ForEach(x => x.Target = message.Target);
      return response;
    }

    private CommandValidationGroupResult ValidateGroup(MemberInfo member, Message message)
    {
      var validators = member.GetCustomAttributes()
        .Where(x=>typeof(ICommandPropertyAttribute).IsAssignableFrom(x.GetType()))
        .Cast<ICommandPropertyAttribute>();

      var validationResult = new CommandValidationGroupResult();
      validationResult.IsValid = true;
      foreach(var validator in validators)
      {
        validator.Validate(member.DeclaringType, message);
      }
      validationResult.Value = message.Text;
      return validationResult;
    }

    private ResponsePacket BuildErrorResponsePacket(CommandCreationResult commandCreationInfo, Message message)
    {
      return BuildErrorResponsePacket(commandCreationInfo.Errors, message);
    }
    private ResponsePacket BuildErrorResponsePacket(List<string> errors, Message message)
    {
      return null;
    }
    private ResponsePacket BuildErrorResponsePacket(string error, Message message)
    {
      return null;
    }

    private void SetPropertyValue(Object target, string name, Object value)
    {
      target.GetType().InvokeMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty, Type.DefaultBinder, target, new Object[] { value });
    }

    private ResponsePacket ExecuteCommand(Object target, Type commandType, Message message)
    {

      var method = commandType.GetMethods()
        .Single(x=> {
          var attribute = x.GetCustomAttribute<Command>();
          if(attribute != null)
            return attribute.Name == message.SubCommand;
          return false;
        });

      var result = method.Invoke(target, new Object[] {});
      
      var response = result as ResponsePacket;
      if(response == null)
      {
        return BuildErrorResponsePacket($"Command {message.Command} Did not return type ResponsePacket.", message);
      }
      return response;
    }
    private CommandCreationResult CreateCommandInstance(Type commandType, Message message)
    {
      //Create and instance of the command type
      var command = Activator.CreateInstance(commandType);
      var validationErrors = new List<string>();

      //Loop through the properties and populate data 
      foreach (var member in commandType.GetMembers().Where(x => x.MemberType == MemberTypes.Property))
      {
        if (member.GetCustomAttributes().Any(x => (x as ICommandPropertyAttribute) != null))
        {
          //Check all the validators on the property and verify they are valid before populating.
          var result = ValidateGroup(member, message);
          if (result.IsValid)
          {
            SetPropertyValue(command, member.Name, result.Value);
            continue;
          }
          validationErrors.Add(result.ErrorMessage);
        }
      }
      return new CommandCreationResult
      {
        Command = command,
        Errors = validationErrors
      };
    }

    private Message RecompileCommandArgs(Message message, List<string> commandNames)
    {
      foreach (var subCommand in commandNames.OrderByDescending(x => x.Length))
      {
        if(message.Text.ToLower().StartsWith(message.Command))
        {
          message.Text = message.Text.Remove(0, message.Command.Length + 1).Trim();
          message.SubCommand = subCommand;
          break;
        }
      }
      return message;
    }

    private List<string> GetCommandNames(Type commandType)
    {
      return commandType.GetMethods()
       .Select(x => x.GetCustomAttribute(typeof(Command)) as Command)
       .Where(x => x != null)
       .Select(x => x?.Name??"")
       .ToList();
    }
  }
}