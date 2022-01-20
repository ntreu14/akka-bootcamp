﻿using System;
using Akka.Actor;
using Akka.Actor.Setup;
using Akka.Configuration;
using Akka.Util;

namespace WinTail
{
	#region Program
	class Program
	{
		public static ActorSystem MyActorSystem;

		static async Task Main(string[] args)
		{
			// initialize MyActorSystem
			MyActorSystem = ActorSystem.Create("MyActorSystem");
			PrintInstructions();

			// time to make your first actors!
			var consoleWriterActor = MyActorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()));
			var consoleReaderActor = MyActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(consoleWriterActor)));


			// tell console reader to begin
			//YOU NEED TO FILL IN HERE
			consoleReaderActor.Tell("start");

			// blocks the main thread from exiting until the actor system is shut down
			//MyActorSystem.Settings.CoordinatedShutdownTerminateActorSystem = false;
			await MyActorSystem.Terminate();  //.AwaitTermination();
		}

		private static void PrintInstructions()
		{
			Console.WriteLine("Write whatever you want into the console!");
			Console.Write("Some lines will appear as");
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.Write(" red ");
			Console.ResetColor();
			Console.Write(" and others will appear as");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write(" green! ");
			Console.ResetColor();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("Type 'exit' to quit this application at any time.\n");
		}
	}
	#endregion
}