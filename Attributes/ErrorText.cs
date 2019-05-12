namespace Digman.Io.IrcBalistic.Attributes
{
  public class ErrorText
  {
    private readonly string _value;
    public ErrorText(string errorText)
    {
      _value = errorText;
    }
    public string Value => _value;
  }
}