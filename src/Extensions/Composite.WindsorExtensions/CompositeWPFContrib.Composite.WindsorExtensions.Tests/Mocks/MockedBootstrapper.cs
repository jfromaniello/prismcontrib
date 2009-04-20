using System.Windows;
using Castle.Windsor;
using Microsoft.Practices.Composite.Logging;
using Microsoft.Practices.Composite.Modularity;

namespace CompositeWPFContrib.Composite.WindsorExtensions.Tests.Mocks
{
    public class MockedBootstrapper : WindsorBootstrapper
    {
        public MockWindsorContainer MockContainer = new MockWindsorContainer();
        public MockModuleEnumerator ModuleEnumerator = new MockModuleEnumerator();
        public MockLoggerAdapter Logger = new MockLoggerAdapter();

        protected override IWindsorContainer CreateContainer()
        {
            return this.MockContainer;
        }

        protected override IModuleEnumerator GetModuleEnumerator()
        {
            return ModuleEnumerator;
        }

        protected override ILoggerFacade LoggerFacade
        {
            get { return Logger; }
        }

        protected override DependencyObject CreateShell()
        {
            return null;
        }
    }
}
