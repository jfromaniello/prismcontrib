using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompositeWPFContrib.Composite.Events;
using Microsoft.Practices.Composite.Events;
using System.Threading;
using CompositeWPFContrib.Composite.Tests.Mocks;

namespace CompositeWPFContrib.Composite.Tests.Events
{
    [TestClass]
    public class CompositeEventFixture
    {
        [TestMethod]
        public void CanSubscribeAndRaiseEvent()
        {
            TestableCompositeSyncContextEvent<string> compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();
            bool published = false;
            compositeSyncContextEvent.Subscribe(delegate { published = true; }, ThreadOption.PublisherThread, true, delegate { return true; });
            compositeSyncContextEvent.Publish(null);

            Assert.IsTrue(published);
        }

        [TestMethod]
        public void CanSubscribeAndRaiseCustomEvent()
        {
            var customEvent = new TestableCompositeSyncContextEvent<Payload>();
            Payload payload = new Payload();
            Payload received = null;
            customEvent.Subscribe(delegate(Payload args) { received = args; });

            customEvent.Publish(payload);

            Assert.AreSame(received, payload);
        }

        [TestMethod]
        public void CanHaveMultipleSubscribersAndRaiseCustomEvent()
        {
            var customEvent = new TestableCompositeSyncContextEvent<Payload>();
            Payload payload = new Payload();
            Payload received1 = null;
            Payload received2 = null;
            customEvent.Subscribe(delegate(Payload args) { received1 = args; });
            customEvent.Subscribe(delegate(Payload args) { received2 = args; });

            customEvent.Publish(payload);

            Assert.AreSame(received1, payload);
            Assert.AreSame(received2, payload);
        }

        [TestMethod]
        public void SubscriberReceivesNotificationOnSyncContextThread()
        {
            TestableCompositeSyncContextEvent<string> compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();
            // Sets up its own simulated UI thread
            SynchronizationContextMock mockContext = new SynchronizationContextMock();
            int calledThreadId = -1;
            int subscribeThreadId = -1;
            ManualResetEvent completedEvent = new ManualResetEvent(false);
            bool completed = false;
            string receivedPayload = null;

            SendOrPostCallback subscribeCB = arg =>
                {
                    subscribeThreadId = Thread.CurrentThread.ManagedThreadId;
                    compositeSyncContextEvent.Subscribe(delegate(string args)
                    {
                        calledThreadId = Thread.CurrentThread.ManagedThreadId;
                        receivedPayload = args;
                        completed = true;
                        completedEvent.Set();
                    }, ThreadOption.SubscriberAffinityThread,false);
                };

            mockContext.Send(subscribeCB, null);
            compositeSyncContextEvent.Publish("Test Payload");
            completedEvent.WaitOne(5000);
            completed = true;

            Assert.AreEqual(subscribeThreadId, calledThreadId);
            Assert.AreSame("Test Payload", receivedPayload);
        }



        [TestMethod]
        public void SubscriberReceivesNotificationOnDifferentThread()
        {
            TestableCompositeSyncContextEvent<string> compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();
            int calledThreadId = -1;
            ManualResetEvent completeEvent = new ManualResetEvent(false);
            compositeSyncContextEvent.Subscribe(delegate
            {
                calledThreadId = Thread.CurrentThread.ManagedThreadId;
                completeEvent.Set();
            }, ThreadOption.BackgroundThread);

            compositeSyncContextEvent.Publish(null);
            completeEvent.WaitOne(5000, false);
            Assert.AreNotEqual(Thread.CurrentThread.ManagedThreadId, calledThreadId);
        }

        [TestMethod]
        public void PayloadGetPassedInBackgroundHandler()
        {
            var customEvent = new TestableCompositeSyncContextEvent<Payload>();
            Payload payload = new Payload();
            ManualResetEvent backgroundWait = new ManualResetEvent(false);

            Payload backGroundThreadReceived = null;
            customEvent.Subscribe(delegate(Payload passedPayload)
            {
                backGroundThreadReceived = passedPayload;
                backgroundWait.Set();
            }, ThreadOption.BackgroundThread, true);

            customEvent.Publish(payload);
            bool eventSet = backgroundWait.WaitOne(5000, false);
            Assert.IsTrue(eventSet);
            Assert.AreSame(backGroundThreadReceived, payload);
        }

        [TestMethod]
        public void SubscribeTakesExecuteDelegateThreadOptionAndFilter()
        {
            TestableCompositeSyncContextEvent<string> compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();
            string receivedValue = null;
            compositeSyncContextEvent.Subscribe(delegate(string value) { receivedValue = value; });

            compositeSyncContextEvent.Publish("test");

            Assert.AreEqual("test", receivedValue);

        }

        [TestMethod]
        public void FilterEnablesActionTarget()
        {
            TestableCompositeSyncContextEvent<string> compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();
            bool goodFilterPublished = false;
            bool badFilterPublished = false;
            compositeSyncContextEvent.Subscribe(delegate { goodFilterPublished = true; }, ThreadOption.PublisherThread, true, delegate { return true; });
            compositeSyncContextEvent.Subscribe(delegate { badFilterPublished = true; }, ThreadOption.PublisherThread, true, delegate { return false; });

            compositeSyncContextEvent.Publish("test");

            Assert.IsTrue(goodFilterPublished);
            Assert.IsFalse(badFilterPublished);

        }

        [TestMethod]
        public void SubscribeDefaultsThreadOptionAndNoFilter()
        {
            TestableCompositeSyncContextEvent<string> compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();
            int calledThreadID = -1;

            compositeSyncContextEvent.Subscribe(delegate { calledThreadID = Thread.CurrentThread.ManagedThreadId; });

            compositeSyncContextEvent.Publish("test");

            Assert.AreEqual(Thread.CurrentThread.ManagedThreadId, calledThreadID);
        }

        [TestMethod]
        public void ShouldUnsubscribeFromPublisherThread()
        {
            var compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();

            Action<string> actionEvent = delegate(string args) { };
            compositeSyncContextEvent.Subscribe(
                actionEvent,
                ThreadOption.PublisherThread);

            Assert.IsTrue(compositeSyncContextEvent.Contains(actionEvent));
            compositeSyncContextEvent.Unsubscribe(actionEvent);
            Assert.IsFalse(compositeSyncContextEvent.Contains(actionEvent));
        }

        [TestMethod]
        public void UnsubscribeShouldNotFailWithNonSubscriber()
        {
            TestableCompositeSyncContextEvent<string> compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();

            Action<string> subscriber = delegate { };
            compositeSyncContextEvent.Unsubscribe(subscriber);
        }

        [TestMethod]
        public void ShouldUnsubscribeFromBackgroundThread()
        {
            var compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();

            Action<string> actionEvent = delegate(string args) { };
            compositeSyncContextEvent.Subscribe(
                actionEvent,
                ThreadOption.BackgroundThread);

            Assert.IsTrue(compositeSyncContextEvent.Contains(actionEvent));
            compositeSyncContextEvent.Unsubscribe(actionEvent);
            Assert.IsFalse(compositeSyncContextEvent.Contains(actionEvent));
        }

        [TestMethod]
        public void ShouldUnsubscribeFromSyncContextThread()
        {
            var compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();

            Action<string> actionEvent = delegate(string args) { };
            compositeSyncContextEvent.Subscribe(
                actionEvent,
                ThreadOption.SubscriberAffinityThread);

            Assert.IsTrue(compositeSyncContextEvent.Contains(actionEvent));
            compositeSyncContextEvent.Unsubscribe(actionEvent);
            Assert.IsFalse(compositeSyncContextEvent.Contains(actionEvent));
        }

        [TestMethod]
        public void ShouldUnsubscribeASingleDelegate()
        {
            var compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();

            int callCount = 0;

            Action<string> actionEvent = delegate { callCount++; };
            compositeSyncContextEvent.Subscribe(actionEvent);
            compositeSyncContextEvent.Subscribe(actionEvent);

            compositeSyncContextEvent.Publish(null);
            Assert.AreEqual<int>(2, callCount);

            callCount = 0;
            compositeSyncContextEvent.Unsubscribe(actionEvent);
            compositeSyncContextEvent.Publish(null);
            Assert.AreEqual<int>(1, callCount);
        }

        [TestMethod]
        public void ShouldNotExecuteOnGarbageCollectedDelegateReferenceWhenNotKeepAlive()
        {
            var compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();

            ExternalAction externalAction = new ExternalAction();
            compositeSyncContextEvent.Subscribe(externalAction.ExecuteAction);

            compositeSyncContextEvent.Publish("testPayload");
            Assert.AreEqual("testPayload", externalAction.PassedValue);

            WeakReference actionEventReference = new WeakReference(externalAction);
            externalAction = null;
            GC.Collect();
            Assert.IsFalse(actionEventReference.IsAlive);

            compositeSyncContextEvent.Publish("testPayload");
        }

        [TestMethod]
        public void ShouldNotExecuteOnGarbageCollectedFilterReferenceWhenNotKeepAlive()
        {
            var compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();

            bool wasCalled = false;
            Action<string> actionEvent = delegate { wasCalled = true; };

            ExternalFilter filter = new ExternalFilter();
            compositeSyncContextEvent.Subscribe(actionEvent, ThreadOption.PublisherThread, false, filter.AlwaysTrueFilter);

            compositeSyncContextEvent.Publish("testPayload");
            Assert.IsTrue(wasCalled);

            wasCalled = false;
            WeakReference filterReference = new WeakReference(filter);
            filter = null;
            GC.Collect();
            Assert.IsFalse(filterReference.IsAlive);

            compositeSyncContextEvent.Publish("testPayload");
            Assert.IsFalse(wasCalled);
        }

        [TestMethod]
        public void CanAddDescriptionWhileEventIsFiring()
        {
            var compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();
            Action<string> emptyDelegate = delegate { };
            compositeSyncContextEvent.Subscribe(delegate { compositeSyncContextEvent.Subscribe(emptyDelegate); });

            Assert.IsFalse(compositeSyncContextEvent.Contains(emptyDelegate));

            compositeSyncContextEvent.Publish(null);

            Assert.IsTrue((compositeSyncContextEvent.Contains(emptyDelegate)));
        }

        [TestMethod]
        public void InlineDelegateDeclarationsDoesNotGetCollectedIncorrectlyWithWeakReferences()
        {
            var compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();
            bool published = false;
            compositeSyncContextEvent.Subscribe(delegate { published = true; }, ThreadOption.PublisherThread, false, delegate { return true; });
            GC.Collect();
            compositeSyncContextEvent.Publish(null);

            Assert.IsTrue(published);
        }

        [TestMethod]
        public void ShouldNotGarbageCollectDelegateReferenceWhenUsingKeepAlive()
        {
            var compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();

            var externalAction = new ExternalAction();
            compositeSyncContextEvent.Subscribe(externalAction.ExecuteAction, ThreadOption.PublisherThread, true);

            WeakReference actionEventReference = new WeakReference(externalAction);
            externalAction = null;
            GC.Collect();
            GC.Collect();
            Assert.IsTrue(actionEventReference.IsAlive);

            compositeSyncContextEvent.Publish("testPayload");

            Assert.AreEqual("testPayload", ((ExternalAction)actionEventReference.Target).PassedValue);
        }

        [TestMethod]
        public void RegisterReturnsTokenThatCanBeUsedToUnsubscribe()
        {
            var compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();
            Action<string> action = delegate { };

            var token = compositeSyncContextEvent.Subscribe(action);
            compositeSyncContextEvent.Unsubscribe(token);

            Assert.IsFalse(compositeSyncContextEvent.Contains(action));
        }

        [TestMethod]
        public void ContainsShouldSearchByToken()
        {
            var compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();
            Action<string> action = delegate { };
            var token = compositeSyncContextEvent.Subscribe(action);

            Assert.IsTrue(compositeSyncContextEvent.Contains(token));

            compositeSyncContextEvent.Unsubscribe(action);
            Assert.IsFalse(compositeSyncContextEvent.Contains(token));
        }

        [TestMethod]
        public void SubscribeDefaultsToPublisherThread()
        {
            var compositeSyncContextEvent = new TestableCompositeSyncContextEvent<string>();
            Action<string> action = delegate { };
            var token = compositeSyncContextEvent.Subscribe(action, true);

            Assert.AreEqual(1, compositeSyncContextEvent.BaseSubscriptions.Count);
            Assert.AreEqual(typeof(EventSubscription<string>), compositeSyncContextEvent.BaseSubscriptions.ElementAt(0).GetType());
        }


       
        class ExternalFilter
        {
            public bool AlwaysTrueFilter(string value)
            {
                return true;
            }
        }

        class ExternalAction
        {
            public string PassedValue;
            public void ExecuteAction(string value)
            {
                PassedValue = value;
            }
        }
        class TestableCompositeSyncContextEvent<TPayload> : CompositeEvent<TPayload>
        {
            public ICollection<IEventSubscription> BaseSubscriptions
            {
                get { return base.Subscriptions; }
            }
        }

        class Payload { }
    }
}
