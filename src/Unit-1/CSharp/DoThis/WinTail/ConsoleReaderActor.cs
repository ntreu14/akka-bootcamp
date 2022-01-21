﻿using Akka.Actor;

namespace WinTail
{
	/// <summary>
	/// Actor responsible for reading FROM the console. 
	/// Also responsible for calling <see cref="ActorSystem.Shutdown"/>.
	/// </summary>
	public class ConsoleReaderActor : UntypedActor
	{
		public const string StartCommand = "start";
		public const string ExitCommand = "exit";
		private readonly IActorRef _validationActor;

		public ConsoleReaderActor(IActorRef validationActor)
		{
			_validationActor = validationActor;
		}

		protected override void OnReceive(object message)
		{
			if (message.Equals(StartCommand))
			{
				DoPrintInstructions();
			}
			
			GetAndValidateInput();
		}

		private void GetAndValidateInput()
		{
			var message = Console.ReadLine();
			if (!string.IsNullOrEmpty(message) && string.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
			{
				// shut down the entire actor system (allows the process to exit)
				Context.System.Terminate();
				return;
			}

			// otherwise, just hand message off to validation actor (by telling its actor ref)
			_validationActor.Tell(message);
		}

		private void DoPrintInstructions()
		{
			Console.WriteLine("Write whatever you want into the console!");
			Console.WriteLine("Some entries will pass validation, and some won't...\n\n");
			Console.WriteLine("Type 'exit' to quit this application at any time.\n");
		}
	}
}