using System;

namespace Asperand.IrcBallistic.Worker.Attributes
{
  public class HelpText: Attribute
  {
    private readonly string _text;
    public HelpText(string text)
    {
        _text = text;
    }
    public string Text => _text;
  }
}