using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CompositeWPFContrib.Composite.Wpf.Controls;
using CompositeWPFContrib.Composite.Wpf.Events;
using CompositeWPFContrib.Composite.Wpf.Regions;
using CompositeWPFContrib.Composite.Wpf.Tests.Mocks;
using Microsoft.Practices.Composite.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompositeWPFContrib.Composite.Wpf.Tests.Regions
{
    [TestClass]
    public class OutlookBarControlRegionAdapterFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IfControlIsNotOutlookBarControlThrows()
        {
            var control = new TabControl();
            IRegionAdapter adapter = new TestableOutlookBarControlRegionAdapter();
            adapter.Initialize(control);
        }

        [TestMethod]
        public void AdapterAssociatesOutlookBarControlWithRegion()
        {
            var control = new OutlookBarControl();
            IRegionAdapter adapter = new TestableOutlookBarControlRegionAdapter();

            IRegion region = adapter.Initialize(control);
            Assert.IsNotNull(region);

            Assert.AreSame(control.ItemsSource, region.Views);
        }

        [TestMethod]
        public void ShouldMoveAlreadyExistingContentInControlToRegion()
        {
            var control = new OutlookBarControl();
            var view = new object();
            control.Items.Add(view);
            IRegionAdapter adapter = new TestableOutlookBarControlRegionAdapter();

            var region = (MockRegion)adapter.Initialize(control);

            Assert.AreEqual(1, region.MockViews.Count());
            Assert.AreSame(view, region.MockViews.ElementAt(0));
            Assert.AreSame(view, control.Items[0]);
        }

        [TestMethod]
        public void ControlWithExistingItemSourceThrows()
        {
            var control = new OutlookBarControl() { ItemsSource = new List<string>() };

            IRegionAdapter adapter = new TestableOutlookBarControlRegionAdapter();

            try
            {
                var region = (MockRegion)adapter.Initialize(control);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
                StringAssert.Contains(ex.Message, "ItemsControl's ItemsSource property is not empty.");
            }
        }

        [TestMethod]
        public void ControlWithExistingBindingOnItemsSourceWithNullValueThrows()
        {
            var control = new OutlookBarControl();
            Binding binding = new Binding("Enumerable");
            binding.Source = new SimpleModel() { Enumerable = null };
            BindingOperations.SetBinding(control, ItemsControl.ItemsSourceProperty, binding);

            IRegionAdapter adapter = new TestableOutlookBarControlRegionAdapter();

            try
            {
                var region = (MockRegion)adapter.Initialize(control);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
                StringAssert.Contains(ex.Message, "ItemsControl's ItemsSource property is not empty.");
            }
        }

        [TestMethod]
        public void AdapterSynchronizesOutlookBarControlSelectionWithRegion()
        {
            var control = new OutlookBarControl();
            object model1 = new object();
            object model2 = new object();
            IRegionAdapter adapter = new OutlookBarControlRegionAdapter(new MockEventAggregator());

            var region = adapter.Initialize(control);
            region.Add(model1);
            region.Add(model2);

            Assert.IsFalse(region.ActiveViews.Contains(model2));

            control.SelectedItem = model2;

            Assert.IsTrue(region.ActiveViews.Contains(model2));
            Assert.IsFalse(region.ActiveViews.Contains(model1));

            control.SelectedItem = model1;

            Assert.IsTrue(region.ActiveViews.Contains(model1));
            Assert.IsFalse(region.ActiveViews.Contains(model2));
        }

        [TestMethod]
        public void AdapterDoesNotPreventRegionFromBeingGarbageCollected()
        {
            var control = new OutlookBarControl();
            object model = new object();
            IRegionAdapter adapter = new OutlookBarControlRegionAdapter(new MockEventAggregator());

            var region = adapter.Initialize(control);
            region.Add(model);

            WeakReference regionWeakReference = new WeakReference(region);
            WeakReference controlWeakReference = new WeakReference(control);
            Assert.IsTrue(regionWeakReference.IsAlive);
            Assert.IsTrue(controlWeakReference.IsAlive);

            region = null;
            control = null;
            GC.Collect();
            GC.Collect();

            Assert.IsFalse(regionWeakReference.IsAlive);
            Assert.IsFalse(controlWeakReference.IsAlive);
        }

        [TestMethod]
        public void AdapterDoesNotPreventRegionFromBeingGarbageCollectedWhenSettingItemsSourceToNull()
        {
            var control = new OutlookBarControl();
            object model = new object();
            IRegionAdapter adapter = new OutlookBarControlRegionAdapter(new MockEventAggregator());

            var region = adapter.Initialize(control);
            region.Add(model);

            WeakReference regionWeakReference = new WeakReference(region);
            Assert.IsTrue(regionWeakReference.IsAlive);

            control.ItemsSource = null;
            region = null;
            GC.Collect();
            GC.Collect();

            Assert.IsFalse(regionWeakReference.IsAlive);
        }

        [TestMethod]
        public void AdapterDoesNotPreventControlFromBeingGarbageCollectedWhenSettingItemsSourceToNull()
        {
            var control = new OutlookBarControl();
            object model = new object();
            IRegionAdapter adapter = new OutlookBarControlRegionAdapter(new MockEventAggregator());

            var region = adapter.Initialize(control);
            region.Add(model);

            WeakReference controlWeakReference = new WeakReference(control);
            Assert.IsTrue(controlWeakReference.IsAlive);

            control.ItemsSource = null;
            control = null;
            GC.Collect();
            GC.Collect();

            Assert.IsFalse(controlWeakReference.IsAlive);
        }

        [TestMethod]
        public void RegionDoesNotGetUpdatedIfTheItemsSourceWasManuallyChanged()
        {
            var control = new OutlookBarControl();
            object model = new object();
            IRegionAdapter adapter = new OutlookBarControlRegionAdapter(new MockEventAggregator());

            var region = adapter.Initialize(control);
            region.Add(model);

            control.ItemsSource = null;
            control.Items.Add(model);
            Assert.IsFalse(region.ActiveViews.Contains(model));

            control.SelectedItem = model;

            Assert.IsFalse(region.ActiveViews.Contains(model), "The region should not be updated");
        }

        [TestMethod]
        public void ActivatingTheViewShouldUpdateTheSelectedItem()
        {
            var control = new OutlookBarControl();
            var view1 = new object();
            var view2 = new object();

            IRegionAdapter adapter = new OutlookBarControlRegionAdapter(new MockEventAggregator());

            var region = adapter.Initialize(control);
            region.Add(view1);
            region.Add(view2);

            Assert.AreNotEqual(view1, control.SelectedItem);

            region.Activate(view1);

            Assert.AreEqual(view1, control.SelectedItem);

            region.Activate(view2);

            Assert.AreEqual(view2, control.SelectedItem);
        }

        [TestMethod]
        public void DeactivatingTheSelectedViewShouldUpdateTheSelectedItem()
        {
            var control = new OutlookBarControl();
            var view1 = new object();
            IRegionAdapter adapter = new OutlookBarControlRegionAdapter(new MockEventAggregator());
            var region = adapter.Initialize(control);
            region.Add(view1);

            region.Activate(view1);

            Assert.AreEqual(view1, control.SelectedItem);

            region.Deactivate(view1);

            Assert.AreNotEqual(view1, control.SelectedItem);
        }

        [TestMethod]
        public void SelectingViewInOutlookBarControlPublishOutlookBarEvent()
        {
            var control = new OutlookBarControl();
            DependencyObject model = new DependencyObject();
            MockEventAggregator eventAggregator = new MockEventAggregator();
            MockOutlookBarEvent outlookBarEvent = new MockOutlookBarEvent();
            eventAggregator.AddMapping<OutlookBarEvent>(outlookBarEvent);
            IRegionAdapter adapter = new OutlookBarControlRegionAdapter(eventAggregator);

            var region = adapter.Initialize(control);
            region.Add(model);

            Assert.IsFalse(outlookBarEvent.PublishCalled);
            control.SelectedItem = model;

            Assert.IsTrue(outlookBarEvent.PublishCalled);
            Assert.IsNull(outlookBarEvent.PublishArgumentPayload);
        }

        [TestMethod]
        public void IfOutlookBarMetadataIsSetSelectingViewInOutlookBarControlPublishOutlookBarEventWithCorrectPayload()
        {
            var control = new OutlookBarControl();
            DependencyObject model = new DependencyObject();
            MockEventAggregator eventAggregator = new MockEventAggregator();
            MockOutlookBarEvent outlookBarEvent = new MockOutlookBarEvent();
            eventAggregator.AddMapping<OutlookBarEvent>(outlookBarEvent);
            IRegionAdapter adapter = new OutlookBarControlRegionAdapter(eventAggregator);

            var region = adapter.Initialize(control);
            OutlookBarControl.SetOutlookBarMetadata(model, new OutlookBarMetadata { Payload = "MyPayload" });
            region.Add(model);

            Assert.IsFalse(outlookBarEvent.PublishCalled);
            Assert.IsNull(outlookBarEvent.PublishArgumentPayload);
            control.SelectedItem = model;

            Assert.IsTrue(outlookBarEvent.PublishCalled);
            Assert.IsNotNull(outlookBarEvent.PublishArgumentPayload);
            Assert.AreEqual("MyPayload", outlookBarEvent.PublishArgumentPayload);
        }

        class SimpleModel
        {
            public IEnumerable Enumerable { get; set; }
        }

        internal class TestableOutlookBarControlRegionAdapter : OutlookBarControlRegionAdapter
        {
            public TestableOutlookBarControlRegionAdapter()
                : base(new MockEventAggregator())
            {

            }
            private readonly MockRegion region = new MockRegion();

            protected override IRegion CreateRegion()
            {
                return region;
            }
        }

        internal class MockOutlookBarEvent : OutlookBarEvent
        {
            public bool PublishCalled;
            public string PublishArgumentPayload;
            public override void Publish(string payload)
            {
                PublishCalled = true;
                PublishArgumentPayload = payload;
            }
        }
    }
}