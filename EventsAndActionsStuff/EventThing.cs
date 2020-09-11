using System;

namespace EventsAndActionsStuff
{
	public class EventThing : IDisposable
	{
		public event EventHandler MyEvent;

		public void Dispose()
		{
			SetEventNull();
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
