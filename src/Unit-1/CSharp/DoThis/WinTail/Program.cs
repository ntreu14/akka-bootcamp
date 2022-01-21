using Akka.Actor;

namespace WinTail
{
	class Program
	{
		private static ActorSystem? _myActorSystem;

		static void Main(string[] args)
		{
			// initialize MyActorSystem
			_myActorSystem = ActorSystem.Create("MyActorSystem");

			// create actors
			var consoleWriterProps = Props.Create<ConsoleWriterActor>();
			var consoleWriterActor = _myActorSystem.ActorOf(consoleWriterProps, "consoleWriterActor");

			var validationActorProps = Props.Create(() => new ValidationActor(consoleWriterActor));
			var validationActor = _myActorSystem.ActorOf(validationActorProps, "validationActor");

			var consoleReaderProps = Props.Create<ConsoleReaderActor>(validationActor);
			var consoleReaderActor = _myActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");


			// tell console reader to begin
			consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

			// blocks the main thread from exiting until the actor system is shut down
			_myActorSystem.WhenTerminated.Wait();
		}
	}
}