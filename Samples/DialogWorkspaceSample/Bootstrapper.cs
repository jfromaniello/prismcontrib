using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.UnityExtensions;
using Microsoft.Practices.Composite.Modularity;

namespace DialogWorkspaceSample
{
    /// <summary>
    /// Bootstrapper for the application
    /// </summary>
    public class Bootstrapper: UnityBootstrapper
    {
        /// <summary>
        /// Creates the shell for the application
        /// </summary>
        /// <returns></returns>
        protected override System.Windows.DependencyObject CreateShell()
        {
            Shell window = Container.Resolve<Shell>();
            window.Show();

            return window;
        }

        /// <summary>
        /// Retrieves the module enumerator for the application
        /// </summary>
        /// <returns></returns>
        protected override Microsoft.Practices.Composite.Modularity.IModuleEnumerator GetModuleEnumerator()
        {
            return new StaticModuleEnumerator().AddModule(typeof(DialogModule));
        }
    }
}
