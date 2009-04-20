using DSASample.Infrastructure.Constants;
using DSASample.Restaurants.Module.DSA.MenuServiceClient;
using DSASample.Restaurants.Module.Views.MenuItemsView;
using DSASample.Restaurants.Module.Views.RestaurantsView;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;

namespace DSASample.Restaurants.Module
{
    public class RestaurantModule : IModule
    {
        public RestaurantModule(IRegionManager regionManager, IUnityContainer container)
        {
            RegionManager = regionManager;
            Container = container;
        }

        public void Initialize()
        {
            RegisterTypesAndServices();

            IRegion leftRegion = RegionManager.Regions[RegionNames.LeftRegion];
            IRegion rightRegion = RegionManager.Regions[RegionNames.RightRegion];
            rightRegion.Add(Container.Resolve<MenuItemsView>());
            leftRegion.Add(Container.Resolve<RestaurantsView>());
        }

        private void RegisterTypesAndServices()
        {
            Container.RegisterType<Agent>(new ContainerControlledLifetimeManager());
        }

        public IUnityContainer Container { get; private set; }
        public IRegionManager RegionManager { get; private set; }
    }
}