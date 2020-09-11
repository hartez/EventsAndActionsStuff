using System;

namespace EventsAndActionsStuff
{
	public class ActionThing : IDisposable
	{
		public event Action<ActionThing> MyEvent;

		public void Dispose()
		{
			SetActionNull();
		}

		public void OnMyEvent()
		{
			MyEvent?.Invoke(this);
		}

		public void SetActionNull()
		{
			MyEvent = null;
		}
	}
}
