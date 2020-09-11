using System;

namespace EventsAndActionsStuff
{
	public class ActionEvent : IDisposable
	{
		public event Action<ActionEvent> MyEvent;

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
