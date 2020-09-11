using System;

namespace EventsAndActionsStuff
{
	public class EventThing : IDisposable
	{
		public event EventHandler MyEvent;

		public void Dispose()
		{
			MyEvent = null;
		}

		public void OnMyEvent()
		{
			MyEvent?.Invoke(this, new EventArgs());
		}

		public void SetEventNull()
		{
			MyEvent = null;
		}
	}
}
