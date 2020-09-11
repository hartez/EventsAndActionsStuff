using System;

namespace EventsAndActionsStuff
{
	public class ActionWithBetterEncapsulation : IDisposable
	{
		private Action<ActionWithBetterEncapsulation> myEvent;

		public Action<ActionWithBetterEncapsulation> MyEvent
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
