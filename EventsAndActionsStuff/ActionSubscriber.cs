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

		public ActionSubscriber(ActionThing at)
		{
			Interlocked.Increment(ref Count);
			at.MyEvent += HandleAction;
			Console.WriteLine($"ActionSubscriber created.");
		}

		private void HandleAction(ActionThing obj)
		{
			Console.WriteLine($"{nameof(ActionThing)} MyEvent raised.");
		}

		~ActionSubscriber()
		{
			Interlocked.Decrement(ref Count);
			Console.WriteLine($"ActionSubscriber finalized.");
		}
	}
}
