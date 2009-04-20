using System.Windows;
using AvalonDock;
using AvalonDockRegionAdapterSample.Views;
using CompositeWPFContrib.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.UnityExtensions;

namespace AvalonDockRegionAdapterSample
{
    /// <summary>
    /// created 18.02.2009 by Markus Raufer
    /// </summary>
    public class Bootstrapper : UnityBootstrapper
    {
        protected override IModuleCatalog GetModuleCatalog()
        {
            var catalog = new ModuleCatalog();
            catalog.AddModule((typeof(MainModule)));

            return catalog;
        }

        protected override DependencyObject CreateShell()
        {
            var shell = Container.Resolve<Shell>();
            shell.Show();

            return shell;
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var regionAdapterMappings = Container.TryResolve<RegionAdapterMappings>();
            if (regionAdapterMappings != null)
            {
                regionAdapterMappings.RegisterMapping(typeof(DockablePane), this.Container.Resolve<DockablePaneRegionAdapter>());
                regionAdapterMappings.RegisterMapping(typeof(DocumentPane), this.Container.Resolve<DocumentPaneRegionAdapter>());
            }

            return base.ConfigureRegionAdapterMappings();
        }
    }
}
