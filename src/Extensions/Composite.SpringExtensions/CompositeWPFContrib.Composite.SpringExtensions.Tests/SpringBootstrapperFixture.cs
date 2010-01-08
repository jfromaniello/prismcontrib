using System;
using CompositeWPFContrib.Composite.SpringExtensions.Tests.Mocks;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompositeWPFContrib.Composite.SpringExtensions.Tests
{
    /// <summary>
    /// Test fixture for the Spring bootstrapper class
    /// </summary>
    [TestClass]
    public class SpringBootstrapperFixture
    {
        /// <summary>
        /// Tries to create the bootstrapper and run it
        /// </summary>
        [TestMethod]
        public void RunDefaultBootstrapper()
        {
            DerivedBootstrapper testSubject = new DerivedBootstrapper();
            testSubject.Run();

            Assert.IsTrue(testSubject.IsInitializeModulesCalled);
            Assert.IsTrue(testSubject.IsCreateContainerCalled);
            Assert.IsTrue(testSubject.IsCreateShellCalled);
            Assert.IsTrue(testSubject.IsGetModuleEnumeratorCalled);
            Assert.IsTrue(testSubject.IsConfigureContainerCalled);
            Assert.IsTrue(testSubject.IsConfigureRegionAdapterMappingsCalled);
        }

        /// <summary>
        /// Makes the bootstrap class concrete instead of abstract
        /// </summary>
        class DerivedBootstrapper : SpringBootstrapper
        {
#pragma warning disable 1591

            public bool IsCreateShellCalled { get; set; }
            public bool IsGetModuleEnumeratorCalled { get; set; }
            public bool IsConfigureContainerCalled { get; set; }
            public bool IsConfigureRegionAdapterMappingsCalled { get; set; }
            public bool IsInitializeModulesCalled { get; set; }
            public bool IsCreateContainerCalled { get; set; }

#pragma warning restore 1591

            /// <summary>
            /// Creates a mockup shell
            /// </summary>
            /// <returns></returns>
            protected override System.Windows.DependencyObject CreateShell()
            {
                IsCreateShellCalled = true;

                return new MockShellWindow();
            }

            /// <summary>
            /// Retrieves an empty static module enumerator
            /// </summary>
            /// <returns></returns>
            protected override IModuleCatalog GetModuleEnumerator()
            {
                IsGetModuleEnumeratorCalled = true;

                return new StaticModuleEnumerator();
            }

            protected override void ConfigureContainer()
            {
                IsConfigureContainerCalled = true;

                base.ConfigureContainer();
            }

            protected override Microsoft.Practices.Composite.Wpf.Regions.RegionAdapterMappings ConfigureRegionAdapterMappings()
            {
                IsConfigureRegionAdapterMappingsCalled = true;

                return base.ConfigureRegionAdapterMappings();
            }

            protected override void InitializeModules()
            {
                IsInitializeModulesCalled = true;

                base.InitializeModules();
            }

            protected override Spring.Context.IApplicationContext CreateContainer()
            {
                IsCreateContainerCalled = true;

                return base.CreateContainer();
            }
        }
    }
}
