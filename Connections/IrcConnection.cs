using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Digman.Io.IrcBalistic.Abstracts;
using Digman.Io.IrcBalistic.Classes;
using Digman.Io.IrcBalistic.Serialization;

namespace Digman.Io.IrcBalistic.Connections
{
  public class IrcConnection : Connection
  {
    public string Nickname => _nickname;
    public string Channel => _channel;
    private string _nickname;
    private string _channel;
    public string _ident;
    private StreamWriter _writer;
    private StreamReader _reader;
    private IrcSerializer _serializer;
    private TcpClient _tcpConnection;
    private string User = "USER IRCbot 0 * :IRCbot";
    public override string Name => $"IRC-{_channel}-{_nickname}";

    public IrcConnection(string nickname, string channel, Action<Message> callBack, ConnectionConfig config) : base(callBack, config)
    {
      _nickname = nickname;
      _channel = channel;
      _serializer = new IrcSerializer(this);
    }
    public override void SendMessage(ResponsePacket response)
    {
      foreach (var line in response.Responses)
      {
        var serializedResponse = _serializer.SerializeResponse(line);
        WriteMessage(serializedResponse);
      }
    }

    public void CloseConnection(Thread thread)
    {
      WriteMessage($"{_ident} QUIT :I'll Show myself out. ");
      DisposeConnection();
      thread.Join();
    }

    public override void Listener()
    {

      try
      {
        InitConnection();
        string inputLine;
        while (Open)
        {
          while ((inputLine = _reader.ReadLine()) != null)
          {
            Console.WriteLine(inputLine);
            var message = _serializer.DeserializeMessage(inputLine);
            CallBack.Invoke(message);
          }

        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
        Thread.Sleep(5000);
        if (Open)
        {
          Listener();
          DisposeConnection();
        }
      }
    }


    private void InitConnection()
    {
      _tcpConnection = new TcpClient("irc.freenode.net", 6667);
      var stream = _tcpConnection.GetStream();
      _reader = new StreamReader(stream);
      _writer = new StreamWriter(stream);
      WriteMessage($"NICK {_nickname}");
      WriteMessage(User);

      var currentLine = string.Empty;
      var identFound = false;
      var channelJoined = false;
      while (!identFound && !channelJoined)
      {
        while ((currentLine = _reader.ReadLine()) != null && !(identFound && channelJoined))
        {
          Console.WriteLine(currentLine);
          string[] splitInput = currentLine.Split(' ');

          if (currentLine.StartsWith($":{_nickname}!"))
          {
            _ident = currentLine.Split(' ')[0];
            identFound = true;
          }
          else if (splitInput[1] == "001")
          {
            WriteMessage($"JOIN {_channel}");
            channelJoined = true;
          }
        }
      }
    }

    public void WriteMessage(string message)
    {
      _writer.WriteLine(message);
      _writer.Flush();
    }

    public User GetUserByName(string username)
    {
      return null;
    }

    private void DisposeConnection()
    {
      _writer.Close();
      _writer.Dispose();
      _reader.Close();
      _reader.Dispose();
      _tcpConnection.Close();
      _tcpConnection.Dispose();
    }
  }
}
