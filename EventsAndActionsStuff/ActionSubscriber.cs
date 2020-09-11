using System;
using System.Threading;

namespace EventsAndActionsStuff
{
	public class ActionSubscriber
	{
		public static int Count;

		public static void ResetCount() 
		{
			Count = 0;
		}

		public ActionSubscriber(ActionEvent at)
		{
			Interlocked.Increment(ref Count);
			at.MyEvent += HandleAction;
			Console.WriteLine($"ActionSubscriber created.");
		}

		private void HandleAction(ActionEvent obj)
		{
			Console.WriteLine($"{nameof(ActionEvent)} MyEvent raised.");
		}

		~ActionSubscriber()
		{
			Interlocked.Decrement(ref Count);
			Console.WriteLine($"ActionSubscriber finalized.");
		}
	}
}
