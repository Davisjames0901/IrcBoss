using System;

namespace Digman.Io.IrcBalistic.Attributes
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