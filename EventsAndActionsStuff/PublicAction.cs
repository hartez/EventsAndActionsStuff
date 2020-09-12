using System;

namespace EventsAndActionsStuff
{
	public class PublicAction : IDisposable
	{
		public Action<PublicAction> MyEvent { get; set; }

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
