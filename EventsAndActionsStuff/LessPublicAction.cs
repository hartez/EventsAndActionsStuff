using System;

namespace EventsAndActionsStuff
{
	public class LessPublicAction : IDisposable
	{
		private Action<LessPublicAction> myEvent;

		public Action<LessPublicAction> MyEvent
		{
			get
			{
				return myEvent;
			}
			set
			{
				myEvent += value;
			}
		}

		public void Dispose()
		{
			SetActionNull();
		}

		public void SetActionNull()
		{
			myEvent = null;
		}

		public void OnMyEvent()
		{
			MyEvent?.Invoke(this);
		}
	}
}
