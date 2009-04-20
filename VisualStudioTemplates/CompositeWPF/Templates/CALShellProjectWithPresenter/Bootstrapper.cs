using System.Windows;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.UnityExtensions;

namespace $safeprojectname$
{
    internal class Bootstrapper : UnityBootstrapper
    {
        protected override IModuleEnumerator GetModuleEnumerator()
        {
            return new StaticModuleEnumerator();
        }


        protected override void ConfigureContainer()
        {
            Container.RegisterType<IShellView, Shell>();

            base.ConfigureContainer();
        }

        protected override DependencyObject CreateShell()
        {
            ShellPresenter presenter = Container.Resolve<ShellPresenter>();
            IShellView view = presenter.View;
            view.ShowView();
            return view as DependencyObject;
        }
    }
}
