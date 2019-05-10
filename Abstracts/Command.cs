using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digman.Io.IrcBalistic.Abstracts
{
    public class Command
    {
        public Command(string description, Func<string, string, Connection, string> callBack, params string[] requiredPermissions)
        {
            Description = description;
            CallBack = callBack;
            RequiredPermissions = requiredPermissions;
        }
        public string Description { get; set; }
        public Func<string, string, Connection, string> CallBack { get; set; }
        public string[] RequiredPermissions { get; set; }

        public bool HasPermission(string role)
        {
            if (RequiredPermissions == null || RequiredPermissions.Length == 0)
            {
                return true;
            }
            return RequiredPermissions.Contains(role);
        }
    }
}
