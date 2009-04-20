using DILight;
using Prism.Interfaces;
using Prism.Services;
using PrismImageSearch.Modules.Search;
using PrismImageSearch.Modules.ResultBrowser;
using PrismImageSearch.Modules.ResultDetails;
using PrismImageSearch.Infrastructure;
using Prism;
using System.Windows.Controls;
using Prism.Regions;

namespace PrismImageSearch
{
    public class Bootstrapper
    {
        IContainer container;

        public void Initialize()
        {
            InitializeContainer();
            RegisterGlobalServices();
            RegisterRegionTypes();
            InitializeShell();
            InitializeModules();
        }

        private void InitializeContainer()
        {
            container = new Container();
            container.RegisterInstance<IContainer>(container);
            container.RegisterInstance<IPrismContainer>(new DILightPrismWrapper(container));
            PrismContainerProvider.Provider = container.Resolve<IPrismContainer>();
        }

        private void InitializeShell()
        {
            Shell shell = new Shell();
            App.Current.RootVisual = shell;
        }

        private void RegisterGlobalServices()
        {
            container.RegisterInstance<IRegionManagerService>(new RegionManagerService());
        }

        private void RegisterRegionTypes()
        {
            // Register regions
            container.RegisterType<IRegion<Panel>, PanelRegion>();
            container.RegisterType<IRegion<ItemsControl>, ItemsControlRegion>();
        }

        private void InitializeModules()
        {
            IModule searchModule = container.Resolve<SearchModule>();
            IModule browserModule = container.Resolve<ResultBrowserModule>();
            IModule detailsModule = container.Resolve<ResultDetailsModule>();

            searchModule.Initialize();
            browserModule.Initialize();
            detailsModule.Initialize();
        }
    }
}
