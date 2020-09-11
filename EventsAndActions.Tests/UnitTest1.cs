using EventsAndActionsStuff;
using NUnit.Framework;
using System;

namespace EventsAndActions.Tests
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
			ActionSubscriber.ResetCount();
		}

		[Test]
		public void TestActionNull()
		{
			ActionThing athing = new ActionThing();

			DoStuff(athing);

			Assert.AreEqual(ActionSubscriber.Count, 1);

			athing.SetActionNull(); // Should clear out the list, removing all refs to subscribers

			GC.Collect();
			GC.WaitForPendingFinalizers();

			Assert.AreEqual(ActionSubscriber.Count, 0, "Setting action to null should have cleared sub list");

			athing.OnMyEvent();
		}

		[Test]
		public void TestActionNotNull()
		{
			ActionThing athing = new ActionThing();

			DoStuff(athing);

			Assert.AreEqual(ActionSubscriber.Count, 1);

			GC.Collect();
			GC.WaitForPendingFinalizers();

			Assert.AreEqual(ActionSubscriber.Count, 1, "Action sub list should still hold a subscriber reference"); 

			athing.OnMyEvent();
		}


		[Test]
		public void TestActionDispose()
		{
			using (ActionThing thing = new ActionThing())
			{
				DoStuff(thing);
				Assert.AreEqual(ActionSubscriber.Count, 1);
			}

			GC.Collect();
			GC.WaitForPendingFinalizers();

			Assert.AreEqual(ActionSubscriber.Count, 0, "Dispose should null the Action");
		}

		static void DoStuff(ActionThing thing)
		{
			new ActionSubscriber(thing);
			thing.OnMyEvent();
		}

		[Test]
		public void TestEventNull()
		{
			EventThing thing = new EventThing();

			DoStuff(thing);

			Assert.AreEqual(EventSubscriber.Count, 1);

			thing.SetEventNull(); // Should clear out the list, removing all refs to subscribers

			GC.Collect();
			GC.WaitForPendingFinalizers();

			Assert.AreEqual(EventSubscriber.Count, 0, "Setting event to null should have cleared sub list");

			thing.OnMyEvent();
		}

		[Test]
		public void TestEventNotNull()
		{
			EventThing thing = new EventThing();

			DoStuff(thing);

			Assert.AreEqual(EventSubscriber.Count, 1);

			GC.Collect();
			GC.WaitForPendingFinalizers();

			Assert.AreEqual(EventSubscriber.Count, 1, "Event sub list should still hold a subscriber reference");

			thing.OnMyEvent();
		}

		[Test]
		public void TestEventDispose()
		{
			using (EventThing thing = new EventThing())
			{
				DoStuff(thing);
				Assert.AreEqual(EventSubscriber.Count, 1);
			}

			GC.Collect();
			GC.WaitForPendingFinalizers();

			Assert.AreEqual(EventSubscriber.Count, 0, "Dispose should null the Event");
		}

		static void DoStuff(EventThing thing)
		{
			new EventSubscriber(thing);
			thing.OnMyEvent();
		}
	}
}