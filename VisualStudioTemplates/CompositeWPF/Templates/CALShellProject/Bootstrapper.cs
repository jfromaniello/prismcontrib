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

        protected override DependencyObject CreateShell()
        {
            Shell shell = new Shell();
            shell.Show();

            return shell;
        }
    }
}
