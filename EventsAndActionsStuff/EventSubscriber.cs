using System;
using System.Threading;

namespace EventsAndActionsStuff
{
	public class EventSubscriber
	{
		public static int Count;

		public EventSubscriber(EventThing at)
		{
			Interlocked.Increment(ref Count);
			at.MyEvent += HandleEvent;
			Console.WriteLine($"EventSubscriber created.");
		}

		private void HandleEvent(object sender, EventArgs args)
		{
			Console.WriteLine($"{nameof(EventThing)} MyEvent raised.");
		}

		~EventSubscriber()
		{
			Interlocked.Decrement(ref Count);
			Console.WriteLine($"EventSubscriber finalized.");
		}
	}
}
