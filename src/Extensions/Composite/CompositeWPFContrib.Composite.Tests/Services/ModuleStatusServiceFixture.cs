using System.Collections.ObjectModel;
using CompositeWPFContrib.Composite.Services;
using CompositeWPFContrib.Composite.Tests.Mocks;
using CompositeWPFContrib.Composite.UnityExtensions;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompositeWPFContrib.Composite.Tests.Services
{
    /// <summary>
    /// Summary description for ModuleStatusServiceFixture
    /// </summary>
    [TestClass]
    public class ModuleStatusServiceFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleStatusServiceFixture"/> class.
        /// </summary>
        public ModuleStatusServiceFixture()
        {

        }

        /// <summary>
        /// Checks if the module status service does return the correct result
        /// when no modules are loaded
        /// </summary>
        [TestMethod]
        public void TestGetModuleStatusInformationWithoutLoadedModules()
        {
            StaticModuleEnumerator moduleEnumerator = new StaticModuleEnumerator();
            IUnityContainer container = new UnityContainer();

            moduleEnumerator.AddModule(typeof(ModuleMock));

            // Register the loopback construction used by compositewpf and 
            // inject the unity extension for module tracking
            container.RegisterInstance<IUnityContainer>(container);
            container.AddNewExtension<ModuleTrackingUnityExtension>();

            container.RegisterInstance<IModuleEnumerator>(moduleEnumerator);
            container.RegisterInstance<IModuleStatusService>(container.Resolve<ModuleStatusService>());

            ReadOnlyCollection<ModuleStatusInfo> modules = container.Resolve<IModuleStatusService>().GetModules();

            Assert.IsNotNull(modules);
            Assert.AreEqual(1, modules.Count);
            Assert.AreEqual(ModuleStatus.Unloaded, modules[0].Status);
        }

        /// <summary>
        /// Checks if the module status service does return the correct result
        /// when modules are loaded
        /// </summary>
        [TestMethod]
        public void TestGetModuleStatusInformationWithLoadedModules()
        {
            StaticModuleEnumerator moduleEnumerator = new StaticModuleEnumerator();
            IUnityContainer container = new UnityContainer();

            moduleEnumerator.AddModule(typeof(ModuleMock));

            // Register the loopback construction used by compositewpf and 
            // inject the unity extension for module tracking
            container.RegisterInstance<IUnityContainer>(container);
            container.AddNewExtension<ModuleTrackingUnityExtension>();

            container.RegisterInstance<IModuleEnumerator>(moduleEnumerator);
            container.RegisterInstance<IModuleStatusService>(container.Resolve<ModuleStatusService>());

            ModuleMock module = container.Resolve<ModuleMock>();
            ReadOnlyCollection<ModuleStatusInfo> modules = container.Resolve<IModuleStatusService>().GetModules();

            Assert.IsNotNull(modules);
            Assert.AreEqual(1, modules.Count);
            Assert.AreEqual(ModuleStatus.Loaded, modules[0].Status);
        }
    }
}