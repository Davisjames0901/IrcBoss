namespace Asperand.IrcBallistic.Core.Events
{
  public class MessageResponse
  {
    public string Text { get; set; }
    public bool IsAction { get; set; }
    public string Target { get; set; }
  }
}