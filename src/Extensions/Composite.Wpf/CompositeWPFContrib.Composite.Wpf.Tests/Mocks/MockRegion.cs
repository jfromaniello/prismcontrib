using System;
using Microsoft.Practices.Composite.Regions;

namespace CompositeWPFContrib.Composite.Wpf.Tests.Mocks
{
    class MockRegion : IRegion
    {
        public MockViewsCollection MockViews = new MockViewsCollection();
        public MockViewsCollection MockActiveViews = new MockViewsCollection();

        public IRegionManager Add(object view)
        {
            MockViews.Items.Add(view);
            return null;
        }

        public void Remove(object view)
        {
            throw new NotImplementedException();
        }

        public void Activate(object view)
        {
            throw new NotImplementedException();
        }

        public IRegionManager Add(object view, string viewName)
        {
            throw new NotImplementedException();
        }

        public IRegionManager Add(object view, string viewName, bool createRegionManagerScope)
        {
            throw new NotImplementedException();
        }

        public object GetView(string viewName)
        {
            throw new NotImplementedException();
        }

        public IRegionManager RegionManager { get; set; }

        public IViewsCollection Views
        {
            get { return MockViews; }
        }

        public IViewsCollection ActiveViews
        {
            get { return MockActiveViews; }
        }

        public void Deactivate(object view)
        {
            throw new NotImplementedException();
        }
    }
}