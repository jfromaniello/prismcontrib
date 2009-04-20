using Castle.Windsor;
using CompositeWPFContrib.Composite.WindsorExtensions.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompositeWPFContrib.Composite.WindsorExtensions.Tests
{
    [TestClass]
    public class WindsorContainerExtensionFixture
    {
        [TestMethod]
        public void CanCheckIfTypeIsRegistered()
        {
            var container = new WindsorContainer();

            Assert.IsFalse(container.IsTypeRegistered<IService>());

            container.AddComponent(typeof(MockService).FullName, typeof(IService), typeof(MockService));

            Assert.IsTrue(container.IsTypeRegistered<IService>());
        }

        [TestMethod]
        public void GenericTryResolveShouldResolveServiceIfExists()
        {
            IWindsorContainer container = new WindsorContainer();
            container.RegisterType<IService, MockService>();

            var service = container.TryResolve<IService>();
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void GenericTryResolveShouldReturnNullIfDoesNotExist()
        {
            IWindsorContainer container = new WindsorContainer();

            var service = container.TryResolve<IService>();
            Assert.IsNull(service);
        }

        [TestMethod]
        public void TryResolveShouldReturnNullIfDoesNotExist()
        {
            IWindsorContainer container = new WindsorContainer();

            var service = container.TryResolve(typeof(IService));
            Assert.IsNull(service);
        }

        [TestMethod]
        public void TryResolveShouldResolveServiceIfExists()
        {
            IWindsorContainer container = new WindsorContainer();
            container.RegisterType<IService, MockService>();

            var service = container.TryResolve(typeof(IService));
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void TryResolveShouldWorkWithValueTypes()
        {
            IWindsorContainer container = new WindsorContainer();

            int valueType = container.TryResolve<int>();
            Assert.AreEqual(default(int), valueType);
        }

        [TestMethod]
        public void CanRegisterTypeAsSingleton()
        {
            IWindsorContainer container = new WindsorContainer();
            container.RegisterType<IService, MockService>();

            var firstService = container.Resolve<IService>();
            var secondService = container.Resolve<IService>();

            Assert.IsNotNull(firstService);
            Assert.IsNotNull(secondService);
            Assert.AreSame(firstService, secondService);
        }

        [TestMethod]
        public void CanRegisterTypeAsTransient()
        {
            IWindsorContainer container = new WindsorContainer();
            container.RegisterType<IService, MockService>(false);

            var firstService = container.Resolve<IService>();
            var secondService = container.Resolve<IService>();

            Assert.IsNotNull(firstService);
            Assert.IsNotNull(secondService);
            Assert.AreNotSame(firstService, secondService);
        }

        [TestMethod]
        public void CanResolveNonExistingServiceImplementations()
        {
            IWindsorContainer container = new WindsorContainer();
            var service = WindsorContainerExtensions.Resolve(container, typeof(MockService));

            Assert.IsNotNull(service);
        }
    }
}
