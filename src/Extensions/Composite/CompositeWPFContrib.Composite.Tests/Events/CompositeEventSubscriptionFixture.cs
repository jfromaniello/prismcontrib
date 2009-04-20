using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Microsoft.Practices.Composite.Events;
using CompositeWPFContrib.Composite.Tests.Mocks;
using CompositeWPFContrib.Composite.Events;

namespace CompositeWPFContrib.Composite.Tests.Events
{
    [TestClass]
    public class CompositeEventSubscriptionFixture
    {
        [TestMethod]
        public void ShouldReceiveDelegateOnDifferentThreadIfThreadAffinity()
        {
            int calledThreadId = -1;
            string calledThreadName = string.Empty;
            ManualResetEvent completeEvent = new ManualResetEvent(false);
            Action<object> action = delegate
            {
                calledThreadId = Thread.CurrentThread.ManagedThreadId;
                calledThreadName = Thread.CurrentThread.Name;
                completeEvent.Set();
            };

            IDelegateReference actionDelegateReference = new MockDelegateReference() { Target = action };
            IDelegateReference filterDelegateReference = new MockDelegateReference() { Target = (Predicate<object>)delegate { return true; } };
            SynchronizationContextMock mockSyncContext = new SynchronizationContextMock();
            var eventSubscription = new CompositeEventSubscription<object>(actionDelegateReference, filterDelegateReference, mockSyncContext);

            var publishAction = eventSubscription.GetExecutionStrategy();

            Assert.IsNotNull(publishAction);

            publishAction.Invoke(null);

            completeEvent.WaitOne(5000, false);
            Assert.AreNotEqual(Thread.CurrentThread.ManagedThreadId, calledThreadId);
            Assert.AreEqual("SynchronizationContextMockThread", calledThreadName);
        }

        [TestMethod]
        public void ShouldReceiveDelegateOnSameThreadIfNoThreadAffinity()
        {
            int calledThreadId = -1;
            ManualResetEvent completeEvent = new ManualResetEvent(false);
            Action<object> action = delegate
            {
                calledThreadId = Thread.CurrentThread.ManagedThreadId;
                completeEvent.Set();
            };

            IDelegateReference actionDelegateReference = new MockDelegateReference() { Target = action };
            IDelegateReference filterDelegateReference = new MockDelegateReference() { Target = (Predicate<object>)delegate { return true; } };
            var eventSubscription = new CompositeEventSubscription<object>(actionDelegateReference, filterDelegateReference, null);

            var publishAction = eventSubscription.GetExecutionStrategy();

            Assert.IsNotNull(publishAction);

            publishAction.Invoke(null);

            completeEvent.WaitOne(5000, false);
            Assert.AreEqual(Thread.CurrentThread.ManagedThreadId, calledThreadId);
        }

    }
}
