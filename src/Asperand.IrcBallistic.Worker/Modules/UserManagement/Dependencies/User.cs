using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;

namespace Asperand.IrcBallistic.Worker.Classes
{
  public class User
  {
    public string Name { get; set; }
    public bool IsAuthed { get; set; }
    public string LastMessage { get; set; }
  }
}