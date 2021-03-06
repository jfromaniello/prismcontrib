﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompositeWPFContrib.Composite.UnityExtensions.Tests.Mocks;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Logging;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.UnityExtensions;

namespace CompositeWPFContrib.Composite.UnityExtensions.Tests
{
    [TestClass]
    public class SimpleUnityBootstrapperFixture
    {
        [TestMethod]
        public void CanCreateConcreteBootstrapper()
        {
            new DefaultBootstrapper();
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
            Assert.IsInstanceOfType(container, typeof(UnityContainer));
        }

        [TestMethod]
        public void ShouldCallInitializeModules()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.InitializeModulesCalled);
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
        public void ShouldCallConfigureTypeMappings()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ConfigureContainerCalled);
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
            bootstrapper.ModuleEnumerator = null;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IModuleCatalog");
        }

        [TestMethod]
        public void NotOvewrittenGetModuleEnumeratorThrowsOnDefaultModuleInitialization()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.OverrideGetModuleEnumerator = false;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IModuleCatalog");
        }

        [TestMethod]
        public void GetLoggerShouldHaveDefault()
        {
            var bootstrapper = new DefaultBootstrapper();
            Assert.IsNull(bootstrapper.DefaultLogger);
            bootstrapper.Run();

            Assert.IsNotNull(bootstrapper.DefaultLogger);
            Assert.IsInstanceOfType(bootstrapper.DefaultLogger, typeof(TraceLogger));
        }

        [TestMethod]
        public void NullModuleLoaderThrowsOnDefaultModuleInitialization()
        {
            var bootstrapper = new MockedBootstrapper();

            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleCatalog), bootstrapper.ModuleEnumerator);
            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleLoader), null);

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IModuleLoader");
        }

        [TestMethod]
        public void ShouldRegisterDefaultTypeMappings()
        {
            var bootstrapper = new MockedBootstrapper();

            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleCatalog), bootstrapper.ModuleEnumerator);
            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleLoader), new MockModuleLoader());

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.MockUnityContainer.Instances.ContainsKey(typeof(ILoggerFacade)));
            Assert.AreSame(bootstrapper.Logger, bootstrapper.MockUnityContainer.Instances[typeof(ILoggerFacade)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Instances.ContainsKey(typeof(IUnityContainer)));
            Assert.AreSame(bootstrapper.MockUnityContainer, bootstrapper.MockUnityContainer.Instances[typeof(IUnityContainer)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Types.ContainsKey(typeof(IContainerFacade)));
            Assert.AreEqual(typeof(UnityContainerAdapter), bootstrapper.MockUnityContainer.Types[typeof(IContainerFacade)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Types.ContainsKey(typeof(IModuleLoader)));
            Assert.AreEqual(typeof(ModuleLoader), bootstrapper.MockUnityContainer.Types[typeof(IModuleLoader)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Types.ContainsKey(typeof(IEventAggregator)));
            Assert.AreEqual(typeof(EventAggregator), bootstrapper.MockUnityContainer.Types[typeof(IEventAggregator)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Instances.ContainsKey(typeof(IModuleCatalog)));
            Assert.AreSame(bootstrapper.ModuleEnumerator, bootstrapper.MockUnityContainer.Instances[typeof(IModuleCatalog)]);
        }

        [TestMethod]
        public void ShouldCallGetStartupLoadedModules()
        {
            var bootstrapper = new MockedBootstrapper();

            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleCatalog), bootstrapper.ModuleEnumerator);
            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleLoader), new MockModuleLoader());

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ModuleEnumerator.GetStartupLoadedModulesCalled);
        }

        [TestMethod]
        public void ShouldCallInitializeOnModuleLoader()
        {
            var bootstrapper = new MockedBootstrapper();

            var moduleLoader = new MockModuleLoader();
            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleCatalog), new MockModuleEnumerator());
            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleLoader), moduleLoader);

            bootstrapper.Run();

            Assert.IsTrue(moduleLoader.InitializeCalled);
        }

        [TestMethod]
        public void ShouldCallInitializeOnModuleLoaderWithStartupModules()
        {
            var bootstrapper = new MockedBootstrapper();
            var moduleLoader = new MockModuleLoader();

            bootstrapper.ModuleEnumerator.StartupLoadedModules = new[] { new ModuleInfo("asm", "type", "name") };

            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleCatalog), bootstrapper.ModuleEnumerator);
            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleLoader), moduleLoader);


            bootstrapper.Run();

            Assert.IsNotNull(moduleLoader.InitializeArgumentModuleInfos);
            Assert.AreEqual(1, moduleLoader.InitializeArgumentModuleInfos.Length);
            Assert.AreEqual("name", moduleLoader.InitializeArgumentModuleInfos[0].ModuleName);
        }

        [TestMethod]
        public void ReturningNullContainerThrows()
        {
            var bootstrapper = new MockedBootstrapper();
            bootstrapper.MockUnityContainer = null;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IUnityContainer");
        }

        [TestMethod]
        public void ShouldCallTheMethodsInOrder()
        {
            var bootstrapper = new TestableOrderedBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(CompareOrder("LoggerFacade", "CreateContainer", bootstrapper.OrderedMethodCallList) < 0);
            Assert.IsTrue(CompareOrder("CreateContainer", "ConfigureContainer", bootstrapper.OrderedMethodCallList) < 0);
            Assert.IsTrue(CompareOrder("ConfigureContainer", "GetModuleEnumerator", bootstrapper.OrderedMethodCallList) < 0);
        }

        [TestMethod]
        public void ShouldLogBootstrapperSteps()
        {
            var bootstrapper = new TestableOrderedBootstrapper();
            bootstrapper.Run();
            var messages = bootstrapper.Logger.Messages;

            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Creating Unity container")));
            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Configuring container")));
            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Initializing modules")));
            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Bootstrapper sequence completed")));
        }

        [TestMethod]
        public void ShouldNotRegisterDefaultServicesAndTypes()
        {
            var bootstrapper = new NonconfiguredBootstrapper();
            bootstrapper.Run(false);


            Assert.IsFalse(bootstrapper.HasRegisteredType(typeof(IEventAggregator)));
            Assert.IsFalse(bootstrapper.HasRegisteredType(typeof(IContainerFacade)));
            Assert.IsFalse(bootstrapper.HasRegisteredType(typeof(IModuleLoader)));
        }

        private static int CompareOrder(string firstString, string secondString, IList<string> list)
        {
            return list.IndexOf(firstString).CompareTo(list.IndexOf(secondString));
        }

        private static void AssertExceptionThrownOnRun(SimpleUnityBootstrapper bootstrapper, Type expectedExceptionType, string expectedExceptionMessageSubstring)
        {
            bool exceptionThrown = false;
            try
            {
                bootstrapper.Run();
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
    }

    class NonconfiguredBootstrapper : SimpleUnityBootstrapper
    {
        private MockUnityContainer mockContainer;

        protected override void InitializeModules()
        {
        }

        protected override IUnityContainer CreateContainer()
        {
            mockContainer = new MockUnityContainer();
            return mockContainer;
        }

        public bool HasRegisteredType(Type t)
        {
            return mockContainer.Types.ContainsKey(t);
        }
    }

    class DefaultBootstrapper : SimpleUnityBootstrapper
    {
        public bool GetEnumeratorCalled;
        public bool GetLoggerCalled;
        public bool InitializeModulesCalled;
        public bool ReturnNullDefaultLogger;
        public bool OverrideGetModuleEnumerator = true;
        public IModuleCatalog ModuleEnumerator = new MockModuleEnumerator();
        public ILoggerFacade DefaultLogger;
        public bool ConfigureContainerCalled;

        public IUnityContainer GetBaseContainer()
        {
            return base.Container;
        }

        protected override void ConfigureContainer()
        {
            ConfigureContainerCalled = true;
            base.ConfigureContainer();
        }

        protected override IModuleCatalog GetModuleEnumerator()
        {
            GetEnumeratorCalled = true;
            if (OverrideGetModuleEnumerator)
            {
                return ModuleEnumerator;
            }
            else
            {
                return base.GetModuleEnumerator();
            }
        }

        protected override ILoggerFacade LoggerFacade
        {
            get
            {
                GetLoggerCalled = true;
                if (ReturnNullDefaultLogger)
                {
                    return null;
                }
                else
                {
                    DefaultLogger = base.LoggerFacade;
                    return DefaultLogger;
                }
            }
        }

        protected override void InitializeModules()
        {
            InitializeModulesCalled = true;
            base.InitializeModules();
        }

    }

    class MockedBootstrapper : SimpleUnityBootstrapper
    {
        public MockUnityContainer MockUnityContainer = new MockUnityContainer();
        public MockModuleEnumerator ModuleEnumerator = new MockModuleEnumerator();
        public MockLoggerAdapter Logger = new MockLoggerAdapter();

        protected override IUnityContainer CreateContainer()
        {
            return this.MockUnityContainer;
        }

        protected override IModuleCatalog GetModuleEnumerator()
        {
            return ModuleEnumerator;
        }

        protected override ILoggerFacade LoggerFacade
        {
            get { return Logger; }
        }
    }

    class TestableOrderedBootstrapper : SimpleUnityBootstrapper
    {
        public IList<string> OrderedMethodCallList = new List<string>();
        public MockLoggerAdapter Logger = new MockLoggerAdapter();
        public bool AddCustomTypeMappings;

        protected override IUnityContainer CreateContainer()
        {
            OrderedMethodCallList.Add("CreateContainer");
            return base.CreateContainer();
        }

        protected override ILoggerFacade LoggerFacade
        {
            get
            {
                OrderedMethodCallList.Add("LoggerFacade");
                return Logger;
            }
        }

        protected override IModuleCatalog GetModuleEnumerator()
        {
            OrderedMethodCallList.Add("GetModuleEnumerator");
            return new MockModuleEnumerator();
        }

        protected override void ConfigureContainer()
        {
            OrderedMethodCallList.Add("ConfigureContainer");
            base.ConfigureContainer();
        }

        protected override void InitializeModules()
        {
            OrderedMethodCallList.Add("InitializeModules");
            base.InitializeModules();
        }

    }
}
