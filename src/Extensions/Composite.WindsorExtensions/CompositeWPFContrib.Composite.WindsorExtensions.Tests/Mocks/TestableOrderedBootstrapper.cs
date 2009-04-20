using System.Collections.Generic;
using System.Windows;
using Castle.Windsor;
using Microsoft.Practices.Composite.Logging;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Regions;

namespace CompositeWPFContrib.Composite.WindsorExtensions.Tests.Mocks
{
    public class TestableOrderedBootstrapper : DefaultBootstrapper
    {
        public IList<string> OrderedMethodCallList = new List<string>();
        public MockLoggerAdapter Logger = new MockLoggerAdapter();
        public bool AddCustomTypeMappings;

        protected override IWindsorContainer CreateContainer()
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

        protected override IModuleEnumerator GetModuleEnumerator()
        {
            OrderedMethodCallList.Add("GetModuleEnumerator");
            return new MockModuleEnumerator();
        }

        protected override void ConfigureContainer()
        {
            OrderedMethodCallList.Add("ConfigureContainer");
            if (AddCustomTypeMappings)
            {
                RegisterTypeIfMissing<IRegionManager, MockRegionManager>(true);
            }
            base.ConfigureContainer();
        }

        protected override void InitializeModules()
        {
            OrderedMethodCallList.Add("InitializeModules");
            base.InitializeModules();
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            OrderedMethodCallList.Add("ConfigureRegionAdapterMappings");
            return base.ConfigureRegionAdapterMappings();
        }

        protected override DependencyObject CreateShell()
        {
            OrderedMethodCallList.Add("CreateShell");
            return null;
        }
    }
}
