using System;
using System.Windows;
using System.Windows.Controls;
using CompositeWPFContrib.Composite.Wpf.Infragistics.DockManager.Metadata;
using CompositeWPFContrib.Composite.Wpf.Infragistics.DockManager.Regions;
using CompositeWPFContrib.Composite.Wpf.Infragistics.Extensions;
using Infragistics.Windows.DockManager;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompositeWPFContrib.Composite.Wpf.Infragistics.Tests.DockManager.Regions
{
    /// <summary>
    /// Checks the logic of the <see cref="TabGroupPaneRegionAdapter"/>
    /// </summary>
    [TestClass]
    public class TabGroupPaneRegionAdapterFixture
    {
        XamDockManager xamDockManager;
        SplitPane splitPane1;
        TabGroupPane tabGroupPane;
        Window win;

        [TestInitialize]
        public void TabGroupPaneTestInitialize()
        {
            xamDockManager = new XamDockManager();
            xamDockManager.BeginInit();
            splitPane1 = new SplitPane();

            xamDockManager.Panes.Add(splitPane1);
            tabGroupPane = new TabGroupPane();
            splitPane1.Panes.Add(tabGroupPane);
            xamDockManager.EndInit();
            CreateAndShowWindow();
        }

        [TestCleanup()]
        public void TabGroupPaneTestCleanup()
        {
            splitPane1 = null;
            xamDockManager = null;
            System.Windows.Threading.Dispatcher.CurrentDispatcher.InvokeShutdown();
            CloseAndReleaseWindow();
        }

        private void CloseAndReleaseWindow()
        {
            win.Close();
            win = null;
        }

        private void CreateAndShowWindow()
        {
            win = new Window();
            win.Content = xamDockManager;
            win.Height = 0;
            win.Width = 0;
            win.Left = -500;
            win.Top = -500;
            win.ShowInTaskbar = false;
            win.Show();
        }

        [TestMethod]
        public void AdaptingStandaloneTabGroupPaneThrows()
        {
            try
            {
                TabGroupPane tabGroupPane = new TabGroupPane();
                IRegionAdapter adapter = new TabGroupPaneRegionAdapter();
                IRegion region = adapter.Initialize(tabGroupPane);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
                StringAssert.Contains(ex.Message, "TabGroupPane must be nested in a XamDockManager control");
            }
        }

        [TestMethod]
        public void AdapterAssociatesTabGroupPaneWithRegionViews()
        {
            IRegionAdapter adapter = new TabGroupPaneRegionAdapter();

            IRegion region = adapter.Initialize(tabGroupPane);
            Assert.IsNotNull(region);

            Assert.AreEqual(0, tabGroupPane.Items.Count);
            TestableView view1 = new TestableView("View #1");
            region.Add(view1);

            Assert.AreSame(((ContentPane)tabGroupPane.Items[0]).Content, view1);

            region.Remove(view1);
            Assert.AreEqual(0, tabGroupPane.Items.Count);
        }

        [TestMethod]
        public void ActivatingViewUpdatesTabGroupPaneSelectedItem()
        {
            IRegionAdapter adapter = new TabGroupPaneRegionAdapter();
            IRegion region = adapter.Initialize(tabGroupPane);
            Assert.IsNotNull(region);

            TestableView view1 = new TestableView("View #1");
            region.Add(view1);

            TestableView view2 = new TestableView("View #2");
            region.Add(view2);

            region.Activate(view2);
            Assert.AreSame(((ContentPane)tabGroupPane.SelectedItem).Content, view2);

            region.Activate(view1);
            Assert.AreSame(((ContentPane)tabGroupPane.SelectedItem).Content, view1);
        }

        [TestMethod]
        public void RegionViewMetadataGetsAppliedToContentPane()
        {
            IRegionAdapter adapter = new TabGroupPaneRegionAdapter();
            IRegion region = adapter.Initialize(tabGroupPane);
            Assert.IsNotNull(region);

            TestableView view1 = new TestableView("View #1");
            region.Add(view1);
            region.Activate(view1);
            Assert.AreSame(((ContentPane)tabGroupPane.SelectedItem).Header, view1.GetTabGroupPaneMetadata().Header);

        }

        [TestMethod]
        public void ActivatingContentPaneMakesViewActive()
        {
            IRegionAdapter adapter = new TabGroupPaneRegionAdapter();
            IRegion region = adapter.Initialize(tabGroupPane);
            Assert.IsNotNull(region);

            TestableView view1 = new TestableView("View #1");
            region.Add(view1);
            Assert.IsFalse(view1.IsActive);
            ContentPane contentPane = ((ContentPane)tabGroupPane.Items[0]);
            contentPane.Activate();
            Assert.IsTrue(view1.IsActive);
        }

        [TestMethod]
        public void ActivatingContentPaneAndFloatMakesViewActive()
        {
            IRegionAdapter adapter = new TabGroupPaneRegionAdapter();
            IRegion region = adapter.Initialize(tabGroupPane);
            Assert.IsNotNull(region);

            TestableView view1 = new TestableView("View #1");
            region.Add(view1);
            Assert.IsFalse(view1.IsActive);

            TestableView view2 = new TestableView("View #2");
            region.Add(view2);
            Assert.IsFalse(view2.IsActive);

            ContentPane contentPane1 = ((ContentPane)tabGroupPane.Items[0]);
            contentPane1.Activate();
            Assert.IsTrue(view1.IsActive);
            contentPane1.ExecuteCommand(ContentPaneCommands.ToggleDockedState);
            Assert.IsTrue(view1.IsActive);

            ContentPane contentPane2 = ((ContentPane)tabGroupPane.Items[1]);
            contentPane2.Activate();
            Assert.IsTrue(view2.IsActive);

            contentPane1.Activate();
            Assert.IsTrue(view1.IsActive);
        }

        [TestMethod]
        public void DeactivatingContentPaneMakesViewInactive()
        {
            IRegionAdapter adapter = new TabGroupPaneRegionAdapter();
            IRegion region = adapter.Initialize(tabGroupPane);
            Assert.IsNotNull(region);

            TestableView view1 = new TestableView("View #1");
            region.Add(view1);
            Assert.IsFalse(view1.IsActive);

            TestableView view2 = new TestableView("View #2");
            region.Add(view2);
            Assert.IsFalse(view2.IsActive);

            ContentPane contentPane = ((ContentPane)tabGroupPane.Items[0]);
            contentPane.Activate();

            Assert.IsTrue(view1.IsActive);
            Assert.IsFalse(view2.IsActive);

            contentPane = ((ContentPane)tabGroupPane.Items[1]);
            contentPane.Activate();

            Assert.IsTrue(view2.IsActive);
            Assert.IsFalse(view1.IsActive);
        }

        [TestMethod]
        public void ActivatingViewMakesContentPaneActive()
        {
            IRegionAdapter adapter = new TabGroupPaneRegionAdapter();
            IRegion region = adapter.Initialize(tabGroupPane);
            Assert.IsNotNull(region);

            TestableView view1 = new TestableView("View #1");
            region.Add(view1);

            Assert.IsNull(xamDockManager.ActivePane);
            region.Activate(view1);
            ContentPane contentPane = ((ContentPane)tabGroupPane.Items[0]);
            Assert.AreSame(contentPane, xamDockManager.ActivePane);
        }

        [TestMethod]
        public void ClosingAndReOpeningInstanceofViewDoesNotThrowException()
        {
            IRegionAdapter adapter = new TabGroupPaneRegionAdapter();
            IRegion region = adapter.Initialize(tabGroupPane);
            Assert.IsNotNull(region);

            TestableView view1 = new TestableView("View #1");
            region.Add(view1);

            ContentPane contentPane = view1.Parent as ContentPane;

            contentPane.ExecuteCommand(ContentPaneCommands.Close);

            bool exceptionCaught = false;
            try
            {
                region.Add(view1);
            }
            catch (Exception e)
            {
                throw e;
            }
            
            Assert.IsFalse(exceptionCaught);
        }

        [TestMethod]
        public void ClosingAndReOpeningNewInstanceofViewDoesNotThrowException()
        {
            IRegionAdapter adapter = new TabGroupPaneRegionAdapter();
            IRegion region = adapter.Initialize(tabGroupPane);
            Assert.IsNotNull(region);

            TestableView view1 = new TestableView("View #1");
            region.Add(view1);

            ContentPane contentPane = view1.Parent as ContentPane;

            contentPane.ExecuteCommand(ContentPaneCommands.Close);

            TestableView view2 = new TestableView("View #1");
            bool exceptionCaught = false;
            try
            {
                region.Add(view2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                exceptionCaught = true;
            }
            Assert.IsFalse(exceptionCaught);
        }


        //[TestMethod]
        //public void ClosingPaneDeactivatesView()

        internal class TestableView : ContentControl, IActiveAware
        {
            public TestableView(string header)
            {
                ITabGroupPaneMetadata tabGroupPaneMetadata = new TabGroupPaneMetadata();
                tabGroupPaneMetadata.Header = header;
                this.SetTabGroupPaneMetadata(tabGroupPaneMetadata);
            }


            #region IActiveAware Members

            private bool _isActive;
            public bool IsActive
            {
                get { return _isActive; }
                set
                {
                    if (_isActive != value)
                    {
                        _isActive = value;
                        IsActiveChanged(this, EventArgs.Empty);
                    }
                }
            }

            public event EventHandler IsActiveChanged = delegate { };

            #endregion
        }

    }
}
