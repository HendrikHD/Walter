using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Walter
{
	class IrcBot
	{
		// Irc server to connect
		public static string SERVER = "irc.rizon.net";
		// Irc server's port (6667 is default port)
		private static int PORT = 6667;
		// User information defined in RFC 2812 (Internet Relay Chat: Client Protocol) is sent to irc server
		private static string USER = "USER CSharpBot 8 * :I'm a C# irc bot";
		// Bot's nickname
		private static string NICK = "Walter";
		// Channel to join
		private static string CHANNEL = "#henrikhd";
		// StreamWriter is declared here so that PingSender can access it
		public static StreamWriter writer;

		static void Main(string[] args)
		{
			NetworkStream stream;
			TcpClient irc;
			string inputLine;
			StreamReader reader;
			string nickname;

			try
			{
				irc = new TcpClient(SERVER, PORT);
				stream = irc.GetStream();
				reader = new StreamReader(stream);
				writer = new StreamWriter(stream) { AutoFlush = true };
				// Start PingSender thread
				var ping = new PingSender();
				ping.Start();
				writer.WriteLine(USER);
				writer.WriteLine("NICK " + NICK);
				writer.WriteLine("JOIN " + CHANNEL);

				while(true)
				{
					while((inputLine = reader.ReadLine()) != null)
					{
						if(inputLine.EndsWith("JOIN :" + CHANNEL))
						{
							// Parse nickname of person who joined the channel
							nickname = inputLine.Substring(1, inputLine.IndexOf("!") - 1);
							// Welcome the nickname to channel by sending a notice
							writer.WriteLine("PRIVMSG " + CHANNEL + " :Hi " + nickname + " and welcome to " + CHANNEL + " channel!");
							writer.WriteLine("PRIVMSG " + nickname + " :Hi " + nickname + " and welcome to " + CHANNEL + " channel!");
							// Sleep to prevent excess flood
							Thread.Sleep(2000);
						}
					}

					// Close all streams
					writer.Close();
					reader.Close();
					irc.Close();
				}
			}
			catch(Exception e)
			{
				// Show the exception, sleep for a while and try to establish a new connection to irc server
				Console.WriteLine(e.ToString());
				Thread.Sleep(5000);
				string[] argv = { };
				Main(argv);
			}
		}
	}
}