using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Digman.Io.IrcBalistic.Abstracts;

namespace Digman.Io.IrcBalistic.CommandSets
{
  public class Default : CommandSet
  {
    private ConnectionManager _connectionManager;
    private List<string> roles = new List<string> { "user", "admin" };
    public Default(ConnectionManager connectionManager)
    {
      _connectionManager = connectionManager;
    }

    protected override void GenerateCommands()
    {
      PrefixCommands.Add("ls", new Command("Lists all current commands you have access to", (x, y, z) => ListCommands(y, z)));
      PrefixCommands.Add("addusr", new Command("Adds a new user", (x, y, z) => AddUser(x, z), "admin"));
      PrefixCommands.Add("rmusr", new Command("Removes an existing user", (x, y, z) => RemoveUser(x, y, z), "admin"));
      PrefixCommands.Add("help", new Command("If you can see this text you know how to use it. Try it on another command....", (x, y, z) => Help(x, y, z)));
      PrefixCommands.Add("ignore", new Command("Ignores the specified user", (x, y, z) => IgnoreUser(x, y, z), "admin"));
      PrefixCommands.Add("unignore", new Command("Unignores the specified user", (x, y, z) => UnignoreUser(x, z), "admin"));
      PrefixCommands.Add("echo", new Command("Echos the input", (x, y, z) => x));
      PrefixCommands.Add("lsusr", new Command("Lists the users and their permissions", (x, y, z) => string.Join("; ",z.Users.Select(u=> $"{u.Key}: {u.Value}"))));
      PrefixCommands.Add("lsignore", new Command("Unignores the specified user", (x, y, z) => string.Join(" ,", z.IgnoreUsers)));
      PrefixCommands.Add("derpify", new Command("", (x,y,z)=>z.UsersLastMessage.ContainsKey(x.ToLower())?$"?derpify {z.UsersLastMessage[x.ToLower()]}": null));
      //PrefixCommands.Add("derpify", new Command("", (x,y,z)=>$"!derpify {x}"));
    }


    private string AddUser(string args, Connection connection)
    {
      var argsArray = args.Split(' ');
      if (argsArray.Length == 1 && !string.IsNullOrWhiteSpace(argsArray[0]))
      {
        return AddUserRole(connection, args, "user");
      }
      else if (argsArray.Length == 2 && roles.Contains(argsArray[1]))
      {
        return AddUserRole(connection, argsArray[0], argsArray[1]);
      }
      return "I dont know what you want..";
    }

    private string AddUserRole(Connection connection, string user, string role)
    {
      user = user.ToLower();
      if (connection.Users.ContainsKey(user))
      {
        return "User is already added.";
      }
      connection.Users.Add(user, role);
      return $"Welcome {role} {user}!";
    }

    private string RemoveUser(string args, string user, Connection connection)
    {
      if (args.ToLower() == "digman" || args.ToLower() == "asperand")
      {
        return $"?mock {user}";
      }
      if (connection.Users.ContainsKey(args))
      {
        connection.Users.Remove(args);
        return "Done";
      }
      return "No such user.";
    }

    private string IgnoreUser(string args, string user, Connection connection)
    {
      args = args.ToLower();
      if (args == "digman" || args == "asperand")
      {
        return $"?mock {user}";
      }
      connection.IgnoreUsers.Add(args);
      return "Done";
    }
    private string UnignoreUser(string args, Connection connection)
    {
      if (connection.IgnoreUsers.Contains(args))
      {
        connection.IgnoreUsers.Remove(args);
        return "Done";
      }
      return "No such ignored user.";
    }

    private string ListCommandSets(string userRole, Connection connection)
    {
      return string.Join(" ,", connection.Commands.SelectMany(x => x.PrefixCommands.Where(y => y.Value.HasPermission(userRole)).Select(z => z.Key)));
    }

    private string ListCommands(string user, Connection connection)
    {
      var commands = connection.Commands.SelectMany(x => x.PrefixCommands);
      var accessableCommands = commands.Where(x => x.Value.HasPermission(connection.GetRole(user)));
      return string.Join(", ", accessableCommands.Select(x => x.Key));
    }
    private string Help(string message, string user, Connection connection)
    {
      var commands = connection.Commands.SelectMany(x => x.PrefixCommands);
      var accessableCommands = commands.Where(x => x.Value.HasPermission(connection.GetRole(user)));
      if (accessableCommands.Any(x => x.Key == message.ToLower()))
      {
        var TargetCommand = accessableCommands.SingleOrDefault(x => x.Key == message.ToLower());
        return $"{TargetCommand.Key}: {TargetCommand.Value.Description}";
      }
      return "Sorry, Either you dont have access command or it doesnt exist.";
    }

  }
}
