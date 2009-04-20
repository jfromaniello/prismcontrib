using System;
using CompositeWPFContrib.Composite.Services;

namespace CompositeWPFContrib.Composite.UnityExtensions.Tests.Mocks
{
    /// <summary>
    /// Mockup implementation for the module status service. Use this
    /// to check if the loaded modules are registered properly
    /// </summary>
    public class ModuleStatusServiceMock : IModuleStatusService
    {
        public static bool IsModuleRegistered;
        public static Type LastRegisteredModule;

        #region IModuleStatusService Members

        /// <summary>
        /// Registers a loaded module type. This is used by the module tracking
        /// unity extension, but can also be used by any other IoC container to register
        /// loaded module types
        /// </summary>
        /// <param name="moduleType">Type information of the module that is loaded</param>
        public void RegisterLoadedModule(Type moduleType)
        {
            IsModuleRegistered = true;
            LastRegisteredModule = moduleType;
        }

        /// <summary>
        /// This implementation does not work for this mockup
        /// </summary>
        /// <returns></returns>
        public System.Collections.ObjectModel.ReadOnlyCollection<ModuleStatusInfo> GetModules()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}