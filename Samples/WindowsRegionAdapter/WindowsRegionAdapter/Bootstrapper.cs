using Microsoft.Practices.Composite.UnityExtensions;
using Microsoft.Practices.Composite.Wpf.Regions;
using System.Windows;
using Microsoft.Practices.Composite.Modularity;
using WRAExample.MyModule;

namespace WRAExample
{
    using CompositeWPFContrib.Composite.Wpf.Regions;

    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            Shell shell = new Shell();
            shell.Show();
            return shell;
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings regionAdapterMappings = Container.TryResolve<RegionAdapterMappings>();

            if (regionAdapterMappings != null)
            {
                regionAdapterMappings.RegisterMapping(typeof(Window), new WindowRegionAdapter() { WindowStyle = (Style)Application.Current.FindResource("WindowTemplate") });
            }

            return base.ConfigureRegionAdapterMappings();
        }

        protected override IModuleEnumerator GetModuleEnumerator()
        {
            StaticModuleEnumerator enumerator = new StaticModuleEnumerator();
            enumerator.AddModule(typeof(MyModuleInit));
            return enumerator;
        }
    }
}
