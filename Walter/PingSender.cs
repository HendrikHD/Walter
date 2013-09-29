using System;
using System.Threading;

namespace Walter
{
	class PingSender
	{
		private static string PING = "PING :";
		private Thread pingSender;

		public PingSender()
		{
			pingSender = new Thread(new ThreadStart(Run));
		}

		// Starts the thread
		public void Start()
		{
			pingSender.Start();
		}

		// Send PING to irc server every 15 seconds
		public void Run()
		{
			while(true)
			{
				IrcBot.writer.WriteLine(PING + IrcBot.SERVER);
				Thread.Sleep(15000);
			}
		}
	}
}