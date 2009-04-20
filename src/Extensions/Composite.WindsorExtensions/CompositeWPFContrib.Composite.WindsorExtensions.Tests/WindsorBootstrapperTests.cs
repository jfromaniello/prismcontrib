using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Castle.Windsor;
using CompositeWPFContrib.Composite.WindsorExtensions.Tests.Mocks;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompositeWPFContrib.Composite.WindsorExtensions.Tests
{
    [TestClass]
    public class WindsorBootstrapperTests
    {
        [TestMethod]
        public void CanCreateBootstrapper()
        {
            var bs = new DefaultBootstrapper();
            Assert.IsNotNull(bs);
        }

        [TestMethod]
        public void CanRunBootstrapper()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();
        }

        [TestMethod]
        public void ShouldInitializeContainer()
        {
            var bootstrapper = new DefaultBootstrapper();
            var container = bootstrapper.GetBaseContainer();

            Assert.IsNull(container);

            bootstrapper.Run();

            container = bootstrapper.GetBaseContainer();

            Assert.IsNotNull(container);
            Assert.IsInstanceOfType(container, typeof(WindsorContainer));
        }

        [TestMethod]
        public void ShouldCallInitializeModules()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.InitializeModulesCalled);
        }

        [TestMethod]
        public void ShouldRegisterDefaultMappings()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();

            Assert.IsNotNull(bootstrapper.DefaultRegionAdapterMappings);
            Assert.IsNotNull(bootstrapper.DefaultRegionAdapterMappings.GetMapping(typeof(ItemsControl)));
            Assert.IsNotNull(bootstrapper.DefaultRegionAdapterMappings.GetMapping(typeof(ContentControl)));
            Assert.IsNotNull(bootstrapper.DefaultRegionAdapterMappings.GetMapping(typeof(Selector)));
        }

        [TestMethod]
        public void ShouldCallGetLogger()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.GetLoggerCalled);
        }

        [TestMethod]
        public void ShouldCallGetEnumerator()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.GetEnumeratorCalled);
        }

        [TestMethod]
        public void ShouldCallCreateSell()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.CreateShellCalled);
        }

        [TestMethod]
        public void ShouldCallConfigureTypeMappings()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ConfigureContainerCalled);
        }

        [TestMethod]
        public void ShouldCallConfigureRegionAdapterMappings()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ConfigureRegionAdapterMappingsCalled);
        }

        [TestMethod]
        public void NullLoggerThrows()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.ReturnNullDefaultLogger = true;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "ILoggerFacade");
        }

        [TestMethod]
        public void NullModuleEnumeratorThrowsOnDefaultModuleInitialization()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.OverrideGetModuleEnumerator = true;
            bootstrapper.CallBaseModuleInitialization = true;
            bootstrapper.ModuleEnumerator = null;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IModuleEnumerator");
        }

        [TestMethod]
        public void NotOvewrittenGetModuleEnumeratorThrowsOnDefaultModuleInitialization()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.OverrideGetModuleEnumerator = false;
            bootstrapper.CallBaseModuleInitialization = true;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IModuleEnumerator");
        }

        [TestMethod]
        public void GetLoggerShouldHaveDefault()
        {
            var bootstrapper = new DefaultBootstrapper();
            Assert.IsNull(bootstrapper.DefaultLogger);
            bootstrapper.Run();

            Assert.IsNotNull(bootstrapper.DefaultLogger);
            Assert.IsInstanceOfType(bootstrapper.DefaultLogger, typeof(Log4NetLogger));
        }

        [TestMethod]
        public void ShouldAssignRegionManagerToReturnedShell()
        {
            var bootstrapper = new DefaultBootstrapper();
            var shell = new DependencyObject();
            bootstrapper.CreateShellReturnValue = shell;

            Assert.IsNull(RegionManager.GetRegionManager(shell));

            bootstrapper.Run();

            Assert.IsNotNull(RegionManager.GetRegionManager(shell));
        }

        [TestMethod]
        public void ShouldNotFailIfReturnedNullShell()
        {
            var bootstrapper = new DefaultBootstrapper { CreateShellReturnValue = null };
            bootstrapper.Run();
        }

        [TestMethod]
        public void ReturningNullContainerThrows()
        {
            var bootstrapper = new MockedBootstrapper();
            bootstrapper.MockContainer = null;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IWindsorContainer");
        }

        [TestMethod]
        public void ShouldCallTheMethodsInOrder()
        {
            var bootstrapper = new TestableOrderedBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(CompareOrder("LoggerFacade", "CreateContainer", bootstrapper.OrderedMethodCallList) < 0);
            Assert.IsTrue(CompareOrder("CreateContainer", "ConfigureContainer", bootstrapper.OrderedMethodCallList) < 0);
            Assert.IsTrue(CompareOrder("ConfigureContainer", "GetModuleEnumerator", bootstrapper.OrderedMethodCallList) < 0);
            Assert.IsTrue(CompareOrder("GetModuleEnumerator", "ConfigureRegionAdapterMappings", bootstrapper.OrderedMethodCallList) < 0);
            Assert.IsTrue(CompareOrder("ConfigureRegionAdapterMappings", "CreateShell", bootstrapper.OrderedMethodCallList) < 0);
            Assert.IsTrue(CompareOrder("CreateShell", "InitializeModules", bootstrapper.OrderedMethodCallList) < 0);
        }

        [TestMethod]
        public void ShouldLogBootstrapperSteps()
        {
            var bootstrapper = new TestableOrderedBootstrapper();
            bootstrapper.Run();
            var messages = bootstrapper.Logger.Messages;

            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Creating Windsor container")));
            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Configuring container")));
            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Configuring region adapters")));
            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Creating shell")));
            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Initializing modules")));
            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Bootstrapper sequence completed")));
        }

        [TestMethod]
        public void ShouldNotRegisterDefaultServicesAndTypes()
        {
            var bootstrapper = new NonconfiguredBootstrapper();
            bootstrapper.Run(false);


            Assert.IsFalse(bootstrapper.HasRegisteredType(typeof(IEventAggregator)));
            Assert.IsFalse(bootstrapper.HasRegisteredType(typeof(IRegionManager)));
            Assert.IsFalse(bootstrapper.HasRegisteredType(typeof(RegionAdapterMappings)));
            Assert.IsFalse(bootstrapper.HasRegisteredType(typeof(IContainerFacade)));
            Assert.IsFalse(bootstrapper.HasRegisteredType(typeof(IModuleLoader)));
        }

        [TestMethod]
        public void ShoudLogRegisterTypeIfMissingMessage()
        {
            var bootstrapper = new TestableOrderedBootstrapper();
            bootstrapper.AddCustomTypeMappings = true;
            bootstrapper.Run();
            var messages = bootstrapper.Logger.Messages;

            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Type 'IRegionManager' was already registered by the application")));
        }

        [TestMethod]
        public void CanRegisterTypeIfMissingAsTransient()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();

            var service = bootstrapper.Container.TryResolve<IService>();
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void NullModuleLoaderThrowsOnDefaultModuleInitialization()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.CallBaseConfigureContainer = true;
            bootstrapper.CallBaseModuleInitialization = true;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IModuleLoader", false);
        }

        [TestMethod]
        public void ShouldGetStartupModulesFromModuleEnumerator()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.CallBaseConfigureContainer = true;
            bootstrapper.CallBaseModuleInitialization = true;
            bootstrapper.RegisterMockModuleLoader = true;

            bootstrapper.Run(false);

            var enumerator = (MockModuleEnumerator)bootstrapper.ModuleEnumerator;
            Assert.IsTrue(enumerator.GetStartupLoadedModulesCalled);
        }

        [TestMethod]
        public void ShouldCallInitializeOnModuleLoader()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.CallBaseConfigureContainer = true;
            bootstrapper.CallBaseModuleInitialization = true;
            bootstrapper.RegisterMockModuleLoader = true;

            bootstrapper.Run(false);

            var loader = (MockModuleLoader)bootstrapper.ModuleLoader;
            Assert.IsTrue(loader.InitializeCalled);
        }

        #region Helper

        private static int CompareOrder(string firstString, string secondString, IList<string> list)
        {
            return list.IndexOf(firstString).CompareTo(list.IndexOf(secondString));
        }

        private static void AssertExceptionThrownOnRun(WindsorBootstrapper bootstrapper, Type expectedExceptionType, string expectedExceptionMessageSubstring)
        {
            AssertExceptionThrownOnRun(bootstrapper, expectedExceptionType, expectedExceptionMessageSubstring, true);
        }

        private static void AssertExceptionThrownOnRun(WindsorBootstrapper bootstrapper, Type expectedExceptionType, string expectedExceptionMessageSubstring, bool defaultConfig)
        {
            bool exceptionThrown = false;

            try
            {
                bootstrapper.Run(defaultConfig);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(expectedExceptionType, ex.GetType());
                StringAssert.Contains(ex.Message, expectedExceptionMessageSubstring);
                exceptionThrown = true;
            }

            if (!exceptionThrown)
            {
                Assert.Fail("Exception not thrown.");
            }
        }

        #endregion
    }
}
