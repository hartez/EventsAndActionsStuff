using EventsAndActionsStuff;
using NUnit.Framework;
using System;
using System.Diagnostics;

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

		static void DoStuff(EventThing thing)
		{
			new EventSubscriber(thing);
			thing.OnMyEvent();
		}

		static void DoStuff(ActionEvent thing)
		{
			new ActionSubscriber(thing);
			thing.OnMyEvent();
		}

		[Test]
		public void TestActionNull()
		{
			ActionEvent athing = new ActionEvent();

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
			ActionEvent athing = new ActionEvent();

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
			using (ActionEvent thing = new ActionEvent())
			{
				DoStuff(thing);
				Assert.AreEqual(ActionSubscriber.Count, 1);
			}

			GC.Collect();
			GC.WaitForPendingFinalizers();

			Assert.AreEqual(ActionSubscriber.Count, 0, "Dispose should null the Action");
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

		[Test]
		public void ObjectInitializers() 
		{
			bool firstHappened = false;
			bool secondHappened = false;

			var x = new PublicAction() 
			{ 
				// So if we use a publicly accessible Action (without the event keyword),
				// we can do this, which is cool
				MyEvent = (a) => { firstHappened = true; }
			};

			// And still do multiple handlers, like this:
			x.MyEvent += (a) => { secondHappened = true; };

			x.OnMyEvent();

			Assert.True(firstHappened);
			Assert.True(secondHappened);
		}

		[Test]
		public void NoObjectInitializersWithEvents()
		{
			bool firstHappened = false;
			bool secondHappened = false;

			var x = new EventThing();
			// With Events, we required two statements to initialize
			x.MyEvent += (sender, args) => { firstHappened = true; };

			// And still do multiple handlers, like this:
			x.MyEvent += (sender, args) => { secondHappened = true; };

			x.OnMyEvent();

			Assert.True(firstHappened);
			Assert.True(secondHappened);
		}

		[Test]
		public void BadEncapsulation()
		{
			bool firstHappened = false;
			bool secondHappened = false;

			var x = new PublicAction()
			{
				MyEvent = (a) => { firstHappened = true; }
			};

			// And the syntax folks are familiar with still works
			x.MyEvent += (a) => { secondHappened = true; };

			// But if we use a publicly accessible Action (without the event keyword), that means
			// that anyone could nuke our event handlers, like this:
			x.MyEvent = null;

			x.OnMyEvent();

			Assert.False(firstHappened);
			Assert.False(secondHappened);
		} 

		[Test]
		public void BetterEncapsulation()
		{
			bool firstHappened = false;
			bool secondHappened = false;
			bool thirdHappened = false;

			var x = new LessPublicAction()
			{
				MyEvent = (a) => { firstHappened = true; }
			};

			// And the expected syntax still works
			x.MyEvent += (a) => { secondHappened = true; };

			// We can ignore null (in this case, just add it as a null action to the subscriber list)
			// And only let the actual backing property be modified internally during Dispose

			// So we avoid having others nuke our events, which is great
			x.MyEvent = null;

			// Though this is probably going to be confusing; it looks like it should
			// replace the existing Action, but instead it simply adds this one to the list
			x.MyEvent = (a) => { thirdHappened = true; };

			x.OnMyEvent();

			Assert.True(firstHappened);
			Assert.True(secondHappened);
			Assert.True(thirdHappened);
		}

		[Test]
		public void ActionAddRemoveLambda()
		{
			var x = new PublicAction();

			x.MyEvent += (a) => ActionToRemove2(a);
			x.MyEvent -= (a) => ActionToRemove2(a);

			x.OnMyEvent();
		}

		[Test]
		public void ActionAddRemoveMethod()
		{
			var x = new PublicAction();

			x.MyEvent += ActionToRemove2;
			x.MyEvent -= ActionToRemove2;

			x.OnMyEvent();
		}

		[Test]
		public void ActionAddRemoveMethodRef()
		{
			var x = new PublicAction();

			Action<PublicAction> actionToRemove = ActionToRemove2;

			x.MyEvent += actionToRemove;
			x.MyEvent -= actionToRemove;

			x.OnMyEvent();
		}

		[Test]
		public void ActionAddRemoveLocalFunction()
		{
			var x = new PublicAction();

			void actionToRemove(PublicAction a) { ActionToRemove2(a); }

			x.MyEvent += actionToRemove;
			x.MyEvent -= actionToRemove;

			x.OnMyEvent();
		}

		[Test]
		public void ActionAddRemoveAnonymousFunction()
		{
			var x = new PublicAction();

			x.MyEvent += (PublicAction a) => { ActionToRemove2(a); };
			x.MyEvent -= (PublicAction a) => { ActionToRemove2(a); };

			x.OnMyEvent();
		}

		[Test]
		public void BetterEncapsulationFailsRemoval()
		{
			bool firstHappened = false;

			var x = new LessPublicAction()
			{
				MyEvent = (a) => { firstHappened = true; }
			};

			x.MyEvent += ActionToRemove;

			// This will fail - ActionToRemove will still be called
			// Because our hack to disallow Event = null nuking prevents proper
			// updating of the modified invocation list
			x.MyEvent -= ActionToRemove;
			
			x.OnMyEvent();

			Assert.True(firstHappened);
		}

		private void ActionToRemove(LessPublicAction action) 
		{
			Assert.Fail("This should not be called");
		}

		private void ActionToRemove2(PublicAction action)
		{
			Assert.Fail("This should not be called");
		}

		private void HandlerToRemove(object sender, EventArgs e)
		{
			Assert.Fail("This should not be called");
		}

		[Test]
		public void EventAddRemoveLambda()
		{
			var x = new EventThing();

			x.MyEvent += (sender, args) => HandlerToRemove(sender, args);
			x.MyEvent -= (sender, args) => HandlerToRemove(sender, args);

			x.OnMyEvent();
		}

		[Test]
		public void EventAddRemoveMethod()
		{
			var x = new EventThing();

			x.MyEvent += HandlerToRemove;
			x.MyEvent -= HandlerToRemove;

			x.OnMyEvent();
		}

		[Test]
		public void EventAddRemoveMethodRef()
		{
			var x = new EventThing();

			EventHandler handlerToRemove = HandlerToRemove;

			x.MyEvent += handlerToRemove;
			x.MyEvent -= handlerToRemove;

			x.OnMyEvent();
		}

		[Test]
		public void EventAddRemoveLocalFunction()
		{
			var x = new EventThing();

			void handler(object sender, EventArgs args) { HandlerToRemove(sender, args); }

			x.MyEvent += handler;
			x.MyEvent -= handler;

			x.OnMyEvent();
		}

		[Test]
		public void EventAddRemoveAnonymousFunction()
		{
			var x = new EventThing();

			x.MyEvent += (object sender, EventArgs args) => { HandlerToRemove(sender, args); };
			x.MyEvent -= (object sender, EventArgs args) => { HandlerToRemove(sender, args); };

			x.OnMyEvent();
		}
	}
}