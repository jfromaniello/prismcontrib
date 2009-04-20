using System;
using System.Collections.Generic;
using Microsoft.Practices.Composite.Events;

namespace CompositeWPFContrib.Composite.Wpf.Tests.Mocks
{
    public class MockEventAggregator : IEventAggregator
    {
        readonly Dictionary<Type, object> events = new Dictionary<Type, object>();
        public TEventType GetEvent<TEventType>() where TEventType : EventBase
        {
            return (TEventType)events[typeof(TEventType)];
        }

        public void AddMapping<TEventType>(TEventType mockEvent)
        {
            events.Add(typeof(TEventType), mockEvent);
        }
    }
}