using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Composite.Modularity;

namespace CompositeWPFContrib.Composite.Modularity
{
    /// <summary>
    /// <see cref="IModuleEnumerator"/> implementation that composites 
    /// several other <see cref="IModuleEnumerator"/> instances
    /// and makes them available as one <see cref="IModuleEnumerator"/> instance
    /// </summary>
    public class CompositeModuleEnumerator : IModuleEnumerator
    {
        private Collection<IModuleEnumerator> _childEnumerators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeModuleEnumerator"/> class.
        /// </summary>
        public CompositeModuleEnumerator()
        {
            _childEnumerators = new Collection<IModuleEnumerator>();
        }

        /// <summary>
        /// Gets the child enumerators
        /// </summary>
        public Collection<IModuleEnumerator> ChildEnumerators
        {
            get { return _childEnumerators; }

        }

        #region IModuleEnumerator Members

        /// <summary>
        /// Gets a single module by the specified name
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public ModuleInfo GetModule(string moduleName)
        {
            foreach (IModuleEnumerator childEnumerator in _childEnumerators)
            {
                ModuleInfo module = childEnumerator.GetModule(moduleName);

                if (module != null)
                    return module;
            }

            return null;
        }

        /// <summary>
        /// Gets all modules
        /// </summary>
        /// <returns></returns>
        public ModuleInfo[] GetModules()
        {
            List<ModuleInfo> modules = new List<ModuleInfo>();

            foreach (IModuleEnumerator childEnumerator in _childEnumerators)
            {
                modules.AddRange(childEnumerator.GetModules());
            }

            return modules.ToArray();
        }

        /// <summary>
        /// Gets all modules that need to be loaded at startup
        /// </summary>
        /// <returns></returns>
        public ModuleInfo[] GetStartupLoadedModules()
        {
            List<ModuleInfo> modules = new List<ModuleInfo>();

            foreach (IModuleEnumerator childEnumerator in _childEnumerators)
            {
                modules.AddRange(childEnumerator.GetStartupLoadedModules());
            }

            return modules.ToArray();
        }

        #endregion
    }
}
