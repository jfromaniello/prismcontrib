using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Composite.Modularity;

namespace CompositeWPFContrib.Composite.Tests.Mocks
{
    public class ModuleEnumeratorMock : IModuleCatalog
    {
        private Collection<ModuleInfo> _modules;

        /// <summary>
        /// Initializes a new instance of the ModuleEnumeratorMock class.
        /// </summary>
        public ModuleEnumeratorMock(Collection<ModuleInfo> modules)
        {
            _modules = modules;
        }

        #region IModuleCatalog Members

        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns></returns>
        public ModuleInfo GetModule(string moduleName)
        {
            foreach (ModuleInfo module in _modules)
            {
                if (module.ModuleName == moduleName)
                    return module;
            }

            return null;
        }

        /// <summary>
        /// Gets the modules.
        /// </summary>
        /// <returns></returns>
        public ModuleInfo[] GetModules()
        {
            return _modules.ToArray();
        }

        public ModuleInfo[] GetStartupLoadedModules()
        {
            return _modules.Where(m => m.StartupLoaded).ToArray();
        }

        #endregion
    }
}