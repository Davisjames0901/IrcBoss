namespace Digman.Io.IrcBalistic.Classes
{
  public class CommandValidationGroupResult
  {
    public bool IsValid { get; set; }
    public object Value { get; set; }
    public string ErrorMessage { get; set; }
  }
}