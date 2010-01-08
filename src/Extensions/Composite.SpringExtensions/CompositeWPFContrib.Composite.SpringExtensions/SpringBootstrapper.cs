using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Logging;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Regions;
using Spring.Context;
using Spring.Context.Support;
using Spring.Objects.Factory.Support;

namespace CompositeWPFContrib.Composite.SpringExtensions
{
    /// <summary>
    /// Bootloader that uses a spring.NET container for the inversion of control aspects
    /// </summary>
    public abstract class SpringBootstrapper
    {
        private IApplicationContext _container;
        private ILoggerFacade _logger;

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the SpringBootstrapper class.
        /// </summary>
        public SpringBootstrapper()
        {
            _logger = new TraceLogger();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads the application
        /// </summary>
        public void Run()
        {
            _logger.Log("Creating Unity container", Category.Debug, Priority.Low);
            _container = CreateContainer();

            if (Container == null)
            {
                throw new InvalidOperationException(Properties.Resources.NullContainerException);
            }

            _logger.Log("Configuring container", Category.Debug, Priority.Low);

            ConfigureContainer();

            _logger.Log("Configuring region adapters", Category.Debug, Priority.Low);

            ConfigureRegionAdapterMappings();

            _logger.Log("Creating shell", Category.Debug, Priority.Low);
            DependencyObject shell = CreateShell();

            if (shell != null)
            {
                RegionManager.SetRegionManager(shell, (IRegionManager)_container.GetObject("IRegionManager"));
            }

            _logger.Log("Initializing modules", Category.Debug, Priority.Low);
            InitializeModules();

            _logger.Log("Bootstrapper sequence completed", Category.Debug, Priority.Low);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Creates the container required by the application
        /// </summary>
        /// <returns></returns>
        protected virtual IApplicationContext CreateContainer()
        {
            return ContextRegistry.GetContext();
        }

        /// <summary>
        /// Configures the IoC container for use
        /// </summary>
        protected virtual void ConfigureContainer()
        {
            IModuleCatalog enumerator = GetModuleEnumerator();

            if (enumerator != null)
            {
                // Register the module enumerator
                RegisterSingletonInstance("IModuleCatalog", enumerator);
            }

            // Register the container itself as an alias
            RegisterSingletonInstance("IContainerFacade", new SpringContainerAdapter(_container));

            // Register the types required by the application
            RegisterTypeIfMissing("RegionAdapterMappings", typeof(RegionAdapterMappings), true);
            RegisterTypeIfMissing("IEventAggregator", typeof(EventAggregator), true);
            RegisterTypeIfMissing("IModuleLoader", typeof(ModuleLoader), true);

            // Execute some special logic to register the region manager
            RegisterRegionManager();
        }

        /// <summary>
        /// Configures the default region adapter mappings to use in the application, in order
        /// to adapt UI controls defined in XAML to use a region and register it automatically.
        /// May be overwritten in a derived class to add specific mappings required by the application.
        /// </summary>
        /// <returns>The <see cref="RegionAdapterMappings"/> instance containing all the mappings.</returns>
        protected virtual RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings regionAdapterMappings = (RegionAdapterMappings)_container.GetObject("RegionAdapterMappings");

            if (regionAdapterMappings != null)
            {
                regionAdapterMappings.RegisterMapping(typeof(Selector), new SelectorRegionAdapter());
                regionAdapterMappings.RegisterMapping(typeof(ItemsControl), new ItemsControlRegionAdapter());
                regionAdapterMappings.RegisterMapping(typeof(ContentControl), new ContentControlRegionAdapter());
            }

            return regionAdapterMappings;
        }

        /// <summary>
        /// Initializes the modules. May be overwritten in a derived class to use custom 
        /// module loading and avoid using an <seealso cref="IModuleLoader"/> and 
        /// <seealso cref="IModuleCatalog"/>.
        /// </summary>
        protected virtual void InitializeModules()
        {
            IModuleCatalog moduleEnumerator = (IModuleCatalog)_container.GetObject("IModuleCatalog");

            if (moduleEnumerator == null)
            {
                throw new InvalidOperationException(
                    Properties.Resources.NullModuleEnumeratorException);
            }

            IModuleLoader moduleLoader = (IModuleLoader)_container.GetObject("IModuleLoader");
            if (moduleLoader == null)
            {
                throw new InvalidOperationException(
                    Properties.Resources.NullModuleLoaderException);
            }

            ModuleInfo[] moduleInfo = moduleEnumerator.GetStartupLoadedModules();
            moduleLoader.Initialize(moduleInfo);
        }

        /// <summary>
        /// Gets the module enumerator for the application
        /// </summary>
        /// <returns></returns>
        protected virtual IModuleCatalog GetModuleEnumerator()
        {
            return null;
        }

        #endregion

        #region Abstract methods

        protected abstract DependencyObject CreateShell();

        #endregion

        #region Private Methods

        /// <summary>
        /// Registers a type if it doesnt exist already in the container
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="typeToRegister"></param>
        /// <param name="singleton"></param>
        private void RegisterTypeIfMissing(string alias, Type typeToRegister, bool singleton)
        {
            if (!_container.ContainsObjectDefinition(alias))
            {
                DefaultObjectDefinitionFactory definitionFactory = new DefaultObjectDefinitionFactory();
                ObjectDefinitionBuilder builder = ObjectDefinitionBuilder.RootObjectDefinition(definitionFactory, typeToRegister);

                builder.SetSingleton(singleton);

                IConfigurableApplicationContext configurableContext = _container as IConfigurableApplicationContext;
                IObjectDefinitionRegistry definitionRegistry = _container as IObjectDefinitionRegistry;

                // Only try to register the new definition objects when the context can be modified
                if (definitionRegistry != null)
                {
                    definitionRegistry.RegisterObjectDefinition(alias, builder.ObjectDefinition);

                    // Only refresh when possible
                    if (configurableContext != null)
                    {
                        configurableContext.Refresh();
                    }
                }
            }
        }

        /// <summary>
        /// Registers the regions manager
        /// </summary>
        /// <remarks>This custom logic is required because the framework
        /// is kind of limited in the way you can work with spring.</remarks>
        private void RegisterRegionManager()
        {
            DefaultObjectDefinitionFactory definitionFactory = new DefaultObjectDefinitionFactory();
            ObjectDefinitionBuilder builder = ObjectDefinitionBuilder.RootObjectDefinition(definitionFactory, typeof(RegionManager));

            builder.AddConstructorArgReference("RegionAdapterMappings");
            builder.SetSingleton(true);

            IConfigurableApplicationContext configurableContext = _container as IConfigurableApplicationContext;
            IObjectDefinitionRegistry definitionRegistry = _container as IObjectDefinitionRegistry;

            // Only try to register the new definition objects when the context can be modified
            if (definitionRegistry != null)
            {
                definitionRegistry.RegisterObjectDefinition(
                    "IRegionManager", builder.ObjectDefinition);

                // Only refresh when possible
                if (configurableContext != null)
                {
                    configurableContext.Refresh();
                }
            }
        }

        /// <summary>
        /// Registers a new singleton instance with the container
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="instance"></param>
        private void RegisterSingletonInstance(string alias, object instance)
        {
            IConfigurableApplicationContext configurableContext = _container as IConfigurableApplicationContext;

            if (configurableContext != null && !_container.ContainsObjectDefinition(alias))
            {
                configurableContext.ObjectFactory.RegisterSingleton(alias, instance);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the container for the bootloader
        /// </summary>
        protected IApplicationContext Container
        {
            get
            {
                return _container;
            }
        }

        #endregion
    }
}
