﻿using System;
using Akka.Actor;

namespace WinTail
{
	/// <summary>
	/// Actor responsible for serializing message writes to the console.
	/// (write one message at a time, champ :)
	/// </summary>
	public class ConsoleWriterActor : UntypedActor
	{
		protected override void OnReceive(object message)
		{
			if (message is InputError error)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(error.Reason);
			}
			else if (message is InputSuccess msg)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine(msg.Reason);
			}
			else
			{
				Console.WriteLine(message);
			}

			Console.ResetColor();
		}
	}
}