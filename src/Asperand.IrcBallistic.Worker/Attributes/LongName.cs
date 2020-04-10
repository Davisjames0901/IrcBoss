using System;

namespace Asperand.IrcBallistic.Worker.Attributes
{
  public class LongName : Attribute
  {
      readonly string _name;
      
      public LongName(string name)
      {
          _name = name;
      }
      
      public string Name => _name; 
  }
}