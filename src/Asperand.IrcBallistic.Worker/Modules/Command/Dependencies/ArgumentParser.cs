using System.Collections.Generic;
using System.Linq;
using Asperand.IrcBallistic.Worker.Events;
using Asperand.IrcBallistic.Worker.Extensions;

namespace Asperand.IrcBallistic.Worker.Modules.Command.Dependencies
{
    //Todo combine this with the serializer and create a command event type?
    public class ArgumentParser
    {
        public CommandRequest ParseCommandRequest(MessageRequest request)
        {
            var tokens = request.Text.Split(' ');
            var content = string.Join(' ',tokens.Skip(1));
            var commandName = string.Join("", tokens[0].Skip(1));
            return new CommandRequest
            {
                Content = content,
                Flags = GetFlags(content),
                CommandName = commandName,
                Raw = request.Text,
                Target = request.Target,
                RequesterUsername = request.SourceUserName
            };
        }

        private Dictionary<string, string> GetFlags(string line)
        {
            var flags = new Dictionary<string, string>();
            var lineTokens = SplitSpaceIgnoreQuoted(line);
            
            var tempFlag = string.Empty;
            
            foreach (var token in lineTokens)
            {
                if (token.StartsWith('-'))
                {
                    flags.Add(token.Remove(0, 1), string.Empty);
                    tempFlag = token.Remove(0,1);
                }
                else if (!string.IsNullOrWhiteSpace(tempFlag))
                {
                    flags[tempFlag] += token;
                }
            }

            return flags;
        }

        private string GetContent(string text)
        {
            return string.Empty;
        }

        private IEnumerable<string> SplitSpaceIgnoreQuoted(string source)
        {
            var temp = string.Empty;
            var list = new List<string>();
            for (var i = 0; i < source.Length; i++)
            {
                var token = source[i];
                if (token == '"' || token == '\'')
                {
                    list.Add(temp);
                    temp = string.Empty;
                    var inx = source.CharLookAhead(token, i);
                    if (inx != -1)
                    {
                        list.Add(source.Substring(i+1, inx - i-1));
                        i = inx;
                    }
                }
                else if (token == ' ')
                {
                    list.Add(temp);
                    temp = string.Empty;
                }
                else
                {
                    temp += token;
                }
            }
            list.Add(temp);

            return list.Where(x=> !string.IsNullOrWhiteSpace(x));
        }
        
    }
}