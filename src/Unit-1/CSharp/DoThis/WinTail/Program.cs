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

		static void Main(string[] args)
		{
			// initialize MyActorSystem
			MyActorSystem = ActorSystem.Create("MyActorSystem");
			
			var consoleWriterActor = MyActorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()), "consoleWriterActor");
			var consoleReaderActor = MyActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(consoleWriterActor)), "consoleReaderActor");

			// tell console reader to begin
			consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

			// blocks the main thread from exiting until the actor system is shut down
			MyActorSystem.WhenTerminated.Wait();
		}
	}
	#endregion
}