using System;

namespace EventsAndActionsStuff
{
	public class ActionWithBadEncapsulation : IDisposable
	{
		public Action<ActionWithBadEncapsulation> MyEvent { get; set; }

		public void Dispose()
		{
			SetActionNull();
		}

		public void SetActionNull()
		{
			MyEvent = null;
		}

		public void OnMyEvent()
		{
			MyEvent?.Invoke(this);
		}
	}
}
