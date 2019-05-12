// using System;
// using System.Linq;
// using Digman.Io.IrcBalistic.Abstracts;

// namespace Digman.Io.IrcBalistic.CommandSets
// {
//   public class Patterns : CommandSet
//   {
//     private bool trollFilter;
//     protected override void GenerateCommands()
//     {
//       PatternCommands.Add("fuck|shit|bitch|cunt|whore|crap|heck|damnit|damn|hell", new Command("", 
//         (x, y, z) => 
//           trollFilter?
//             string.Join("", x.ToLower().Select((c, i)=>i%2==0?Char.ToUpper(c): c))
//             :null
//         ));
//       PrefixCommands.Add("troll", new Command("Toggles the troll filter.", (x,y,z) => {
//         trollFilter = !trollFilter;
//         return "Troll filter enabled: "+trollFilter;
//       }));
//     }
//   }

// }