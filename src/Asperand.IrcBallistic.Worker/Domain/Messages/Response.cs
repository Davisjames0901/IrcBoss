namespace Asperand.IrcBallistic.Worker.Messages
{
  public class Response
  {
    public string Text { get; set; }
    public bool IsAction { get; set; }
    public string Target { get; set; }
  }
}