using Digman.Io.IrcBalistic.Attributes;
using Digman.Io.IrcBalistic.Classes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Digman.Io.IrcBalistic.Commands
{
  [CommandGroup("echo")]
  [HelpText("Echos the input provided.")]
  public class Echo : ICommand
  {

    [Required]
    [Content]
    public string Input { get; set; }

    [Command]
    public ResponsePacket Execute()
    {
      var response = new Response
      {
        IsAction = Input.StartsWith("/me")
      };

      if(response.IsAction)
      {
        response.Text = Input.Substring(3, Input.Length - 3).Trim();
      }
      else 
      {
        response.Text = Input;
      }

      return new ResponsePacket
      {
        Responses = new List<Response>
        {
          response
        }
      };
    }
  }
}