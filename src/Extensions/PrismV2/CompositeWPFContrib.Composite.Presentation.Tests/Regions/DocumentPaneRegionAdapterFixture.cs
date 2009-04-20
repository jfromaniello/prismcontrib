using AvalonDock;
using CompositeWPFContrib.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompositeWPFContrib.Composite.Presentation.Tests.Regions
{
    /// <summary>
    /// created 18.02.2009 by Markus Raufer
    /// </summary>
    [TestClass]
    public class DocumentPaneRegionAdapterFixture
    {
        [TestMethod]
        public void AdapterAssociatesDocumentPaneWithRegion()
        {
            var documentPane = new DocumentPane();

            IRegionAdapter adapter = new DocumentPaneRegionAdapter(null);
            IRegion region = adapter.Initialize(documentPane, "Region1");
            
            Assert.IsNotNull(region);
        }
    }
}
