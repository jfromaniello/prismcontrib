using System;
using System.Windows;
using Commanding.Modules.Order.PresentationModels;
using Commanding.Modules.Order.Services;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;

namespace Commanding.Modules.Order
{
    public class OrderModule : IModule
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;

        public OrderModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            RegisterResources();
            container.RegisterType<IOrdersRepository, OrdersRepository>(new ContainerControlledLifetimeManager());

            IRegion mainRegion = regionManager.Regions["MainRegion"];
            mainRegion.Add(container.Resolve<OrdersEditorPresentationModel>());

            IRegion globalCommandsRegion = regionManager.Regions["GlobalCommandsRegion"];
            globalCommandsRegion.Add(container.Resolve<OrdersToolbarPresentationModel>());
        }

        private static void RegisterResources()
        {
            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("pack://application:,,,/Commanding.Modules.Order;Component/OrdersResourceDictionary.xaml");
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }
    }
}