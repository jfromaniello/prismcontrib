using System;
using System.Windows;
using Castle.Windsor;

namespace CompositeWPFContrib.Composite.WindsorExtensions.Tests.Mocks
{
    public class NonconfiguredBootstrapper : WindsorBootstrapper
    {
        private WindsorContainer container;

        protected override void InitializeModules()
        {
        }

        protected override IWindsorContainer CreateContainer()
        {
            container = new WindsorContainer();
            return container;
        }

        protected override DependencyObject CreateShell()
        {
            return null;
        }

        public bool HasRegisteredType(Type t)
        {
            return container.Kernel.HasComponent(t);
        }
    }
}
