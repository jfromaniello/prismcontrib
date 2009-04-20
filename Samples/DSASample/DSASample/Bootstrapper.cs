using System.Windows;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.UnityExtensions;
using Microsoft.Practices.SmartClient.DisconnectedAgent;
using Microsoft.Practices.SmartClient.EnterpriseLibrary;
using Microsoft.Practices.Unity;

namespace DSASample
{
    internal class Bootstrapper : UnityBootstrapper
    {
        protected override IModuleEnumerator GetModuleEnumerator()
        {
            return new StaticModuleEnumerator()
                .AddModule(typeof(Restaurants.Module.RestaurantModule));
        }

        protected override DependencyObject CreateShell()
        {
            ShellPresenter presenter = Container.Resolve<ShellPresenter>();
            IShell shell = presenter.Shell;
            shell.Show();

            return shell as DependencyObject;
        }

        protected override void ConfigureContainer()
        {
            Container.RegisterType<IShell, Shell>();

            // Set up request manager.
            RequestManager requestManager = DatabaseRequestManagerIntializer.Initialize();
            requestManager.StartAutomaticDispatch();

            // Add the request queue to the Container. This queue will be used by service agents to enqueue requests.
            Container.RegisterInstance<IRequestQueue>(requestManager.RequestQueue, new ContainerControlledLifetimeManager());

            // Add the connection monitor to the Container. It will be used to determine connectivity status and provide feedback to the user accordingly.
            Container.RegisterInstance<IConnectionMonitor>(requestManager.ConnectionMonitor, new ContainerControlledLifetimeManager());

            base.ConfigureContainer();
        }
    }
}