using System;
using Microsoft.Practices.Composite.Logging;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Composite.UnityExtensions;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Events;
using System.Globalization;
using CompositeWPFContrib.Composite.UnityExtensions.Properties;

namespace CompositeWPFContrib.Composite.UnityExtensions
{
    public abstract class SimpleUnityBootstrapper
    {
        private readonly ILoggerFacade _loggerFacade = new TraceLogger();
        private bool _useDefaultConfiguration = true;

        /// <summary>
        /// Gets the default <see cref="IUnityContainer"/> for the application.
        /// </summary>
        /// <value>The default <see cref="IUnityContainer"/> instance.</value>
        public IUnityContainer Container { get; private set; }

        /// <summary>
        /// Gets the default <see cref="ILoggerFacade"/> for the application.
        /// </summary>
        /// <value>A <see cref="ILoggerFacade"/> instance.</value>
        protected virtual ILoggerFacade LoggerFacade
        {
            get { return _loggerFacade; }
        }

        /// <summary>
        /// Runs the bootstrapper process.
        /// </summary>
        public void Run()
        {
            Run(true);
        }

        /// <summary>
        /// Run the bootstrapper process.
        /// </summary>
        /// <param name="useDefaultConfiguration">If <see langword="true"/>, registers default Composite Application Library services in the container. This is the default behavior.</param>
        public void Run(bool useDefaultConfiguration)
        {
            _useDefaultConfiguration = useDefaultConfiguration;
            ILoggerFacade logger = LoggerFacade;
            if (logger == null)
            {
                throw new InvalidOperationException(Resources.NullLoggerFacadeException);
            }

            logger.Log("Creating Unity container", Category.Debug, Priority.Low);
            Container = CreateContainer();
            if (Container == null)
            {
                throw new InvalidOperationException(Resources.NullUnityContainerException);
            }

            logger.Log("Configuring container", Category.Debug, Priority.Low);

            ConfigureContainer();

            logger.Log("Initializing modules", Category.Debug, Priority.Low);
            InitializeModules();

            logger.Log("Bootstrapper sequence completed", Category.Debug, Priority.Low);
        }

        /// <summary>
        /// Configures the <see cref="IUnityContainer"/>. May be overwritten in a derived class to add specific
        /// type mappings required by the application.
        /// </summary>
        protected virtual void ConfigureContainer()
        {
            Container.RegisterInstance<ILoggerFacade>(LoggerFacade);
            Container.RegisterInstance<IUnityContainer>(Container);
            Container.AddNewExtension<UnityBootstrapperExtension>();

            IModuleCatalog moduleEnumerator = GetModuleEnumerator();
            if (moduleEnumerator != null)
            {
                Container.RegisterInstance<IModuleCatalog>(moduleEnumerator);
            }
            if (_useDefaultConfiguration)
            {
                RegisterTypeIfMissing(typeof(IContainerFacade), typeof(UnityContainerAdapter), true);
                RegisterTypeIfMissing(typeof(IEventAggregator), typeof(EventAggregator), true);
                RegisterTypeIfMissing(typeof(IModuleLoader), typeof(ModuleLoader), true);
            }
        }

        /// <summary>
        /// Initializes the modules. May be overwritten in a derived class to use custom 
        /// module loading and avoid using an <seealso cref="IModuleLoader"/> and 
        /// <seealso cref="IModuleCatalog"/>.
        /// </summary>
        protected virtual void InitializeModules()
        {
            IModuleCatalog moduleEnumerator = Container.TryResolve<IModuleCatalog>();
            if (moduleEnumerator == null)
            {
                throw new InvalidOperationException(Resources.NullModuleEnumeratorException);
            }

            IModuleLoader moduleLoader = Container.TryResolve<IModuleLoader>();
            if (moduleLoader == null)
            {
                throw new InvalidOperationException(Resources.NullModuleLoaderException);
            }

            ModuleInfo[] moduleInfo = moduleEnumerator.GetStartupLoadedModules();
            moduleLoader.Initialize(moduleInfo);
        }

        /// <summary>
        /// Creates the <see cref="IUnityContainer"/> that will be used as the default container.
        /// </summary>
        /// <returns>A new instance of <see cref="IUnityContainer"/>.</returns>
        protected virtual IUnityContainer CreateContainer()
        {
            return new UnityContainer();
        }

        /// <summary>
        /// Returns the module enumerator that will be used to initialize the modules.
        /// </summary>
        /// <remarks>
        /// When using the default initialization behavior, this method must be overwritten by a derived class.
        /// </remarks>
        /// <returns>An instance of <see cref="IModuleCatalog"/> that will be used to initialize the modules.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected virtual IModuleCatalog GetModuleEnumerator()
        {
            return null;
        }

        /// <summary>
        /// Registers a type in the container only if that type was not already registered.
        /// </summary>
        /// <param name="fromType">The interface type to register.</param>
        /// <param name="toType">The type implementing the interface.</param>
        /// <param name="registerAsSingleton">Registers the type as a singleton.</param>
        protected void RegisterTypeIfMissing(Type fromType, Type toType, bool registerAsSingleton)
        {
            ILoggerFacade logger = LoggerFacade;

            if (Container.IsTypeRegistered(fromType))
            {
                logger.Log(
                    String.Format(CultureInfo.CurrentCulture,
                                  Resources.TypeMappingAlreadyRegistered,
                                  fromType.Name), Category.Debug, Priority.Low);
            }
            else
            {
                if (registerAsSingleton)
                {
                    Container.RegisterType(fromType, toType, new ContainerControlledLifetimeManager());
                }
                else
                {
                    Container.RegisterType(fromType, toType);
                }
            }
        }
    }
}
