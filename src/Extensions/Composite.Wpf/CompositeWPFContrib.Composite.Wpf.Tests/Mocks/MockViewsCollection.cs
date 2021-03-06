using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Microsoft.Practices.Composite.Regions;

namespace CompositeWPFContrib.Composite.Wpf.Tests.Mocks
{
    class MockViewsCollection : IViewsCollection
    {
        public ObservableCollection<object> Items = new ObservableCollection<object>();

        public bool Contains(object value)
        {
            return Items.Contains(value);
        }

        public IEnumerator<object> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { Items.CollectionChanged += value; }
            remove { Items.CollectionChanged -= value; }
        }
    }
}