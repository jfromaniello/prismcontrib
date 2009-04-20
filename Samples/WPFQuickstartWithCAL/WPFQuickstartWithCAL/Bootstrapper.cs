using System.Windows;
using WPFQuickstartWithCAL.EmployeeData;
using WPFQuickstartWithCAL.OrganizationChart;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.UnityExtensions;

namespace WPFQuickstartWithCAL
{
    internal class Bootstrapper : UnityBootstrapper
    {
        protected override IModuleEnumerator GetModuleEnumerator()
        {
            return new StaticModuleEnumerator()
                .AddModule(typeof(EmployeeDataModule))
                .AddModule(typeof(OrganizationChartModule));
        }

        protected override DependencyObject CreateShell()
        {
            Shell shell = new Shell();
            shell.Show();

            return shell;
        }
    }
}
