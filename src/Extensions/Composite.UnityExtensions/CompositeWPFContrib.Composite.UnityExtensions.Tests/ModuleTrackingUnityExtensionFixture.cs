using CompositeWPFContrib.Composite.Services;
using CompositeWPFContrib.Composite.UnityExtensions.Tests.Mocks;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompositeWPFContrib.Composite.UnityExtensions.Tests
{
    /// <summary>
    /// Summary description for ModuleTrackingUnityExtension
    /// </summary>
    [TestClass]
    public class ModuleTrackingUnityExtensionFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleTrackingUnityExtensionFixture"/> class.
        /// </summary>
        public ModuleTrackingUnityExtensionFixture()
        {

        }

        /// <summary>
        /// Initializes the testcase
        /// </summary>
        [TestInitialize()]
        public void MyTestInitialize()
        {
            ModuleStatusServiceMock.IsModuleRegistered = false;
            ModuleStatusServiceMock.LastRegisteredModule = null;
        }

        /// <summary>
        /// Checks if initializing a module triggers the module tracking logic
        /// </summary>
        [TestMethod]
        public void TestInitializeModule()
        {
            // Construct a new container and add the mockup to it
            IUnityContainer container = new UnityContainer();
            container.RegisterInstance<IModuleStatusService>(new ModuleStatusServiceMock());

            // Register the extension
            container.AddNewExtension<ModuleTrackingUnityExtension>();

            ModuleMock mockup = container.Resolve<ModuleMock>();

            Assert.IsTrue(ModuleStatusServiceMock.IsModuleRegistered);
            Assert.AreEqual(typeof(ModuleMock), ModuleStatusServiceMock.LastRegisteredModule);
        }

        /// <summary>
        /// Checks if initializing a non-module type does not trigger the module tracking logic
        /// </summary>
        [TestMethod]
        public void TestInitializeNonModuleType()
        {
            // Construct a new container and add the mockup to it
            IUnityContainer container = new UnityContainer();
            container.RegisterInstance<IModuleStatusService>(new ModuleStatusServiceMock());

            // Register the extension
            container.AddNewExtension<ModuleTrackingUnityExtension>();

            GenericMock instance = container.Resolve<GenericMock>();

            Assert.IsFalse(ModuleStatusServiceMock.IsModuleRegistered);
            Assert.IsNull(ModuleStatusServiceMock.LastRegisteredModule);
        }
    }
}