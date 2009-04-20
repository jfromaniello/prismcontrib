using System.Windows;
using Castle.Windsor;
using Microsoft.Practices.Composite.Logging;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Wpf.Regions;

namespace CompositeWPFContrib.Composite.WindsorExtensions.Tests.Mocks
{
    public class DefaultBootstrapper : WindsorBootstrapper
    {
        public bool GetEnumeratorCalled;
        public bool GetLoggerCalled;
        public bool InitializeModulesCalled;
        public bool CreateShellCalled;
        public bool ReturnNullDefaultLogger;
        public bool OverrideGetModuleEnumerator = true;
        public bool CallBaseConfigureContainer = true;
        public bool RegisterMockModuleLoader;
        public bool CallBaseModuleInitialization;
        public IModuleEnumerator ModuleEnumerator = new MockModuleEnumerator();
        public IModuleLoader ModuleLoader = new MockModuleLoader();
        public ILoggerFacade DefaultLogger;
        public RegionAdapterMappings DefaultRegionAdapterMappings;
        public DependencyObject CreateShellReturnValue;
        public bool ConfigureContainerCalled;
        public bool ConfigureRegionAdapterMappingsCalled;

        public IWindsorContainer GetBaseContainer()
        {
            return Container;
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            ConfigureRegionAdapterMappingsCalled = true;
            var regionAdapterMappings = base.ConfigureRegionAdapterMappings();

            DefaultRegionAdapterMappings = regionAdapterMappings;

            return regionAdapterMappings;
        }

        protected override void ConfigureContainer()
        {
            ConfigureContainerCalled = true;

            if (CallBaseConfigureContainer)
                base.ConfigureContainer();

            RegisterTypeIfMissing<IService, MockService>(false);

            if (RegisterMockModuleLoader)
            {
                Container.Kernel.AddComponentInstance(typeof(IModuleLoader).FullName, typeof(IModuleLoader), ModuleLoader);
            }
        }

        protected override IModuleEnumerator GetModuleEnumerator()
        {
            GetEnumeratorCalled = true;
            if (OverrideGetModuleEnumerator)
                return ModuleEnumerator;

            return base.GetModuleEnumerator();
        }

        protected override ILoggerFacade LoggerFacade
        {
            get
            {
                GetLoggerCalled = true;
                if (ReturnNullDefaultLogger)
                    return null;

                DefaultLogger = base.LoggerFacade;
                return DefaultLogger;
            }
        }

        protected override void InitializeModules()
        {
            InitializeModulesCalled = true;

            if (CallBaseModuleInitialization)
                base.InitializeModules();
        }

        protected override DependencyObject CreateShell()
        {
            CreateShellCalled = true;

            return CreateShellReturnValue;
        }
    }
}
