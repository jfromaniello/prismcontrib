using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CompositeWPFContrib.Composite.Wpf.Regions;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompositeWPFContrib.Composite.Wpf.Tests.Regions
{
    /// <summary>
    /// Tests the logic of the <see cref="ToolBarPanelRegionAdapter"/>
    /// </summary>
    [TestClass]
    public class ToolBarPanelRegionAdapterFixture
    {
        /// <summary>
        /// Checks if the <see cref="ToolBarPanelRegionAdapter"/> correctly initializes
        /// when there are no existing controls on the <see cref="ToolBarPanel"/>
        /// </summary>
        [TestMethod]
        public void ToolBarPanelRegionAdapterInitializesWithoutControls()
        {
            ToolBarPanel panel = new ToolBarPanel();
            ToolBarPanelRegionAdapter adapter = new ToolBarPanelRegionAdapter();

            IRegion region = adapter.Initialize(panel);

            Assert.IsNotNull(region);
            Assert.IsInstanceOfType(region, typeof(AllActiveRegion));
        }

        /// <summary>
        /// Checks if <see cref="ToolBarPanelRegionAdapter"/> correctly initializes when
        /// there are already existing controls on the <see cref="ToolBarPanel"/>
        /// </summary>
        [TestMethod]
        public void ToolBarPanelRegionAdapterInitializesWithExistingControls()
        {
            ToolBarPanel panel = new ToolBarPanel();
            ToolBarPanelRegionAdapter adapter = new ToolBarPanelRegionAdapter();

            panel.Children.Add(new ToolBar());
            panel.Children.Add(new ToolBar());

            IRegion region = adapter.Initialize(panel);

            Assert.IsNotNull(region);
            Assert.AreSame(panel.Children[0], region.ActiveViews.First());
        }
    }
}