using System;
using Akka.Actor;

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
		private IActorRef _consoleWriterActor;

		public ConsoleReaderActor(IActorRef consoleWriterActor)
		{
			_consoleWriterActor = consoleWriterActor;
		}

		protected override void OnReceive(object message)
		{
			if (message.Equals(StartCommand))
			{
				DoPrintInstructions();
			}
			else if (message is InputError error)
			{
				_consoleWriterActor.Tell(error);
			}

			GetAndValidateInput();
		}

		private void GetAndValidateInput()
		{
			var message = Console.ReadLine();
			if (string.IsNullOrEmpty(message))
			{
				// signal that the user needs to supply an input, as previously
				// received input was blank
				Self.Tell(new NullInputError("No input received."));
			}
			else if (string.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
			{
				// shut down the entire actor system (allows the process to exit)
				Context.System.Terminate();
			}
			else
			{
				var valid = IsValid(message);
				if (valid)
				{
					_consoleWriterActor.Tell(new InputSuccess("Thank you! Message was valid."));

					Self.Tell(new ContinueProcessing());
				}
				else
				{
					Self.Tell(new ValidationError("Invalid: Input had off number  of characters"));
				}
			}
		}

		private bool IsValid(string message) =>
			message.Length % 2 == 0;

		private void DoPrintInstructions()
		{
			Console.WriteLine("Write whatever you want into the console!");
			Console.WriteLine("Some entries will pass validation, and some won't...\n\n");
			Console.WriteLine("Type 'exit' to quit this application at any time.\n");
		}
	}
}