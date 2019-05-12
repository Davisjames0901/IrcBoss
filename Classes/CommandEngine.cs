using System;
using System.Linq;
using System.Reflection;
using Digman.Io.IrcBalistic.Attributes;
using Digman.Io.IrcBalistic.Commands;

namespace Digman.Io.IrcBalistic.Classes
{
  public class CommandEngine
  {
    public ResponsePacket ExecuteCommand(Message message)
    {
      var type = typeof(ICommand);
      var types = AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(s => s.GetTypes())
          .Where(p => type.IsAssignableFrom(p));
          
      var commandType = types
          .SingleOrDefault(x=>(Attribute.GetCustomAttribute(x, typeof(CommandName)) as CommandName)?.Name == message.Command.ToLower());
      if(commandType == null) 
      {
        return null;
      }
      var command = Activator.CreateInstance(commandType);
      foreach (var member in commandType.GetMembers().Where(x=>x.MemberType == MemberTypes.Property))
      {
        command.GetType().InvokeMember(member.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty, Type.DefaultBinder, command, new Object[]{string.Join(" ", message.Arguments)});
      }
      var response = ((ICommand)command).Execute();
      response.Responses.ForEach(x=>x.Target = message.Target);
      return response;
    }
  }
}