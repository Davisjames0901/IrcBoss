using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Digman.Io.IrcBalistic.Abstracts;
using System.IO;

namespace Digman.Io.IrcBalistic.CommandSets
{
  public class PhraseCommands : CommandSet
  {

    protected override void GenerateCommands()
    {
      LoadPhrases();
    }

    private string LoadPhrases()
    {
      PrefixCommands = new Dictionary<string, Command>();
      PatternCommands = new Dictionary<string, Command>();
      PrefixCommands.Add("rephrase", new Command("Refreshs phrases", (x, y, z) => LoadPhrases(), "admin"));
      PrefixCommands.Add("addphrase", new Command("Adds a new phrase file", (x, y, z) => AddPhrase(x), "admin"));
      PrefixCommands.Add("rmphrase", new Command("Removes a phrase", (x, y, z) => RemovePhrase(x), "admin"));
      PrefixCommands.Add("lsphrase", new Command("Lists phrases", (x, y, z) => ListPhrases(x), "admin"));
      PrefixCommands.Add("catphrase", new Command("Cats to a phrase file", (x, y, z) => CatPhrase(x), "admin"));
      PrefixCommands.Add("mentionphrase", new Command("Registers phrase as a mention phrase", (x, y, z) => RegisterMentionPhrase(x), "admin"));
      PrefixCommands.Add("unmentionphrase", new Command("Registers phrase as a mention phrase", (x, y, z) => RemoveMentionPhrase(x), "admin"));

      var phraseFiles = Directory.GetFiles("./Data");

      foreach (var phrase in phraseFiles)
      {
        var tokens = phrase.Split("/").Last().Split(".");
        if (tokens.Contains("phrase"))
        {
          PrefixCommands.Add(tokens[0], new Command($"Gets a random phrase from file {tokens[0]} :D", (x, y, z) => GetRandomLine(tokens[0])));
        }
        if (tokens.Contains("mention"))
        {
          PatternCommands.Add(tokens[0], new Command($"", (x, y, z) => GetRandomLine(tokens[0])));
        }
      }


      return "Done.";
    }


    private string GetRandomLine(string commandName)
    {
      var rand = new Random(DateTime.Now.Millisecond);
      var lines = GetPhraseLines(commandName);
      if(lines == null || lines.Length < 1)
      {
        return null;
      }

      var randint = rand.Next(0, lines.Count());
      return lines[randint];
    }

    private string AddPhrase(string args)
    {
      if (args.Split(' ').Length > 1)
      {
        return "You cannot have spaces in names.";
      }
      if (string.IsNullOrWhiteSpace(args.Split(' ')[0]))
      {
        return "You must supply a name.";
      }
      if (File.Exists($"./Data/{args.Trim()}.phrase"))
      {
        return "That phrase file already exists. Try catting to it or refreshing phrases.";
      }
      File.WriteAllText($"./Data/{args.Trim()}.phrase", "");
      LoadPhrases();
      return "Done.";
    }


    private string CatPhrase(string args)
    {
      var argTokens = args.Split(' ').ToList();
      if (argTokens.Count <= 1)
      {
        return "Not enough args.";
      }
      var fileName = GetPhraseFileName(argTokens[0]);
      if (fileName == null)
      {
        return "That phrase file doesnt exist. Try creating it first.";
      }
      File.AppendAllText(fileName, string.Join(" ", argTokens.Where((x,y)=> y!=0))+"\n");
      return "Done.";
    }

    private string ListPhrases(string args)
    {
      if (args.Trim() == "-a")
      {
        return string.Join(", ", Directory.GetFiles("./Data"));
      }
      return string.Join(", ", GetPhrases());
    }


    private string RemovePhrase(string args)
    {
      if (PhraseExists(args))
      {
        RemovePhraseAttribute(args, "phrase");
        return "Done.";
      }
      return "That is not a phrase.";
    }

    private string RegisterMentionPhrase(string args)
    {
      if (!PhraseExists(args))
      {
        return "That is not a valid phrase.";
      }
      PatternCommands.Add(args, new Command("", (x, y, z) => GetRandomLine(args)));
      AddPhraseAttribute(args, "mention");
      return "Done.";
    }


    private string RemoveMentionPhrase(string args)
    {
      if (!PhraseExists(args))
      {
        return "That is not a valid phrase.";
      }
      RemovePhraseAttribute(args, "mention");
      return "Done.";
    }

    private string GetPhraseFileName(string phrase)
    {
      return Directory.GetFiles("./Data").FirstOrDefault(x=>x.Split('/').Last().StartsWith(phrase));
    }

    private string[] GetPhrases()
    {
      return Directory.GetFiles("./Data").Select(x=>x.Split('/').Last().Split('.')[0]).ToArray();
    }

    private bool PhraseExists(string phrase)
    {
      return GetPhraseFileName(phrase) != null;
    }

    private void RemovePhraseAttribute(string phrase, string attribute)
    {

      var filename = GetPhraseFileName(phrase);
      var tokens = filename.Split('/').Last().Split('.').ToList();
      if(!tokens.Contains(attribute))
      {
        return;
      }
      tokens.Remove(attribute);
      File.Move(filename, $"./Data/{string.Join(".", tokens)}");
      LoadPhrases();
    }


    private void AddPhraseAttribute(string phrase, string attribute)
    {
      
      var filename = GetPhraseFileName(phrase);
      var tokens = filename.Split('/').Last().Split('.').ToList();
      if(tokens.Contains(attribute))
      {
        return;
      }
      tokens.Add(attribute);
      File.Move(filename, $"./Data/{string.Join(".", tokens)}");
      LoadPhrases();
    }

    private string[] GetPhraseLines(string phrase)
    {
      var filename = GetPhraseFileName(phrase);
      if(filename == null)
      {
        return null;
      }
      return File.ReadAllLines(filename);
    }
  }
}
