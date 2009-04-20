using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Composite.Regions;
using System.Windows;
using Microsoft.Practices.Composite.Wpf.Regions;
using System.Windows.Controls;
using CompositeWPFContrib.Composite.Wpf.Regions;

namespace CompositeWPFContrib.Composite.Wpf.Tests.Regions
{
    /// <summary>
    ///This is a test class for WindowRegionAdapterTest and is intended
    ///to contain all WindowRegionAdapterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WindowRegionAdapterFixture
    {
        Window window;
        WindowRegionAdapter adapter;
        IRegion region;

        [TestInitialize]
        public void Initialize()
        {
            window = new Window();
            adapter = new WindowRegionAdapter();
            region = adapter.Initialize(window);

            window.Show();
        }

        [TestCleanup]
        public void CleanUp()
        {
            window.Close();
        }

        [TestMethod]
        public void WindowRegionAdapterReturnsSingleActiveRegionWhenInitializes()
        {
            Assert.IsNotNull(region);
            Assert.IsInstanceOfType(region, typeof(SingleActiveRegion));
        }

        [TestMethod]
        public void AddedViewIsActived()
        {
            UserControl view = new UserControl();
            region.Add(view);

            Assert.IsTrue(region.Views.Contains(view));
            Assert.IsTrue(region.ActiveViews.Contains(view));
        }

        [TestMethod]
        public void HiddedWindowMakesViewInactive()
        {
            UserControl view = new UserControl();
            region.Add(view);

            Assert.IsTrue(region.ActiveViews.Contains(view));

            Window viewContainerWindow = (Window)view.Parent;
            viewContainerWindow.Hide();

            Assert.IsTrue(region.Views.Contains(view));
            Assert.IsFalse(region.ActiveViews.Contains(view));
        }

        [TestMethod]
        public void ViewIsRemovedWhenWindowCloses()
        {
            UserControl view = new UserControl();
            region.Add(view);

            Assert.IsTrue(region.Views.Contains(view));

            Window viewContainerWindow = (Window)view.Parent;
            viewContainerWindow.Close();

            Assert.IsFalse(region.Views.Contains(view));
        }

        [TestMethod]
        public void LastAddedViewIsActive()
        {
            UserControl firstView = new UserControl();
            region.Add(firstView);

            Assert.IsTrue(region.Views.Contains(firstView));
            Assert.IsTrue(region.ActiveViews.Contains(firstView));

            UserControl secondView = new UserControl();
            region.Add(secondView);

            Assert.IsTrue(region.Views.Contains(secondView));
            Assert.IsFalse(region.ActiveViews.Contains(firstView));
            Assert.IsTrue(region.ActiveViews.Contains(secondView));
        }
    }
}
