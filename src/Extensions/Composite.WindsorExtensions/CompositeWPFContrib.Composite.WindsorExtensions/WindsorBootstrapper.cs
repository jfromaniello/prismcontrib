using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CompositeWPFContrib.Composite.WindsorExtensions.Properties;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Logging;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Regions;

namespace CompositeWPFContrib.Composite.WindsorExtensions
{
    /// <summary>
    /// Bootloader that uses an IWindsorContainer for the inversion of control aspects 
    /// and Log4Net for the logging mechanism.
    /// </summary>
    public abstract class WindsorBootstrapper
    {
        #region Fields

        private readonly ILoggerFacade _loggerFacade = new Log4NetLogger();
        private IWindsorContainer _container;
        private bool _useDefaultConfiguration = true;

        #endregion

        #region Props

        /// <summary>
        /// Gets the default <see cref="IWindsorContainer"/> for the application.
        /// </summary>
        /// <value>The default <see cref="IWindsorContainer"/> instance.</value>
        public IWindsorContainer Container
        {
            get { return _container; }
            private set { _container = value; }
        }

        /// <summary>
        /// Gets the default <see cref="ILoggerFacade"/> for the application.
        /// </summary>
        /// <value>A <see cref="ILoggerFacade"/> instance.</value>
        protected virtual ILoggerFacade LoggerFacade
        {
            get { return _loggerFacade; }
        }

        #endregion

        #region Run

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

            logger.Log("Creating Windsor container", Category.Debug, Priority.Low);
            Container = CreateContainer();
            if (Container == null)
            {
                throw new InvalidOperationException(Resources.NullWindsorContainerException);
            }

            logger.Log("Configuring container", Category.Debug, Priority.Low);

            ConfigureContainer();

            logger.Log("Configuring region adapters", Category.Debug, Priority.Low);

            ConfigureRegionAdapterMappings();

            logger.Log("Creating shell", Category.Debug, Priority.Low);
            DependencyObject shell = CreateShell();

            if (shell != null)
            {
                RegionManager.SetRegionManager(shell, Container.Resolve<IRegionManager>());
            }

            logger.Log("Initializing modules", Category.Debug, Priority.Low);
            InitializeModules();

            logger.Log("Bootstrapper sequence completed", Category.Debug, Priority.Low);
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Configures the <see cref="IWindsorContainer"/>. May be overwritten in a derived class to add specific
        /// type mappings required by the application.
        /// </summary>
        protected virtual void ConfigureContainer()
        {
            Container.Kernel.AddComponentInstance(typeof(ILoggerFacade).FullName, typeof(ILoggerFacade), LoggerFacade);
            Container.Kernel.AddComponentInstance(typeof(IWindsorContainer).FullName, typeof(IWindsorContainer), Container);

            IModuleEnumerator moduleEnumerator = GetModuleEnumerator();
            if (moduleEnumerator != null)
            {
                Container.Kernel.AddComponentInstance(typeof(IModuleEnumerator).FullName, typeof(IModuleEnumerator), moduleEnumerator);
            }

            if (_useDefaultConfiguration)
            {
                RegisterTypeIfMissing<IContainerFacade, WindsorContainerAdapter>(true);
                RegisterTypeIfMissing<IEventAggregator, EventAggregator>(true);
                RegisterTypeIfMissing<RegionAdapterMappings, RegionAdapterMappings>(true);
                RegisterTypeIfMissing<IRegionManager, RegionManager>(true);
                RegisterTypeIfMissing<IModuleLoader, ModuleLoader>(true);
            }
        }

        /// <summary>
        /// Configures the default region adapter mappings to use in the application, in order
        /// to adapt UI controls defined in XAML to use a region and register it automatically.
        /// May be overwritten in a derived class to add specific mappings required by the application.
        /// </summary>
        /// <returns>The <see cref="RegionAdapterMappings"/> instance containing all the mappings.</returns>
        protected virtual RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings regionAdapterMappings = null;

            if (Container.IsTypeRegistered<RegionAdapterMappings>())
            {
                regionAdapterMappings = Container.TryResolve<RegionAdapterMappings>();
                regionAdapterMappings.RegisterMapping(typeof(Selector), new SelectorRegionAdapter());
                regionAdapterMappings.RegisterMapping(typeof(ItemsControl), new ItemsControlRegionAdapter());
                regionAdapterMappings.RegisterMapping(typeof(ContentControl), new ContentControlRegionAdapter());
            }

            return regionAdapterMappings;
        }

        /// <summary>
        /// Initializes the modules. May be overwritten in a derived class to use custom 
        /// module loading and avoid using an <seealso cref="IModuleLoader"/> and 
        /// <seealso cref="IModuleEnumerator"/>.
        /// </summary>
        protected virtual void InitializeModules()
        {
            var moduleEnumerator = Container.TryResolve<IModuleEnumerator>();
            if (moduleEnumerator == null)
            {
                throw new InvalidOperationException(Resources.NullModuleEnumeratorException);
            }

            var moduleLoader = Container.TryResolve<IModuleLoader>();
            if (moduleLoader == null)
            {
                throw new InvalidOperationException(Resources.NullModuleLoaderException);
            }

            ModuleInfo[] moduleInfo = moduleEnumerator.GetStartupLoadedModules();
            moduleLoader.Initialize(moduleInfo);
        }

        /// <summary>
        /// Creates the <see cref="IWindsorContainer"/> that will be used as the default container.
        /// </summary>
        /// <returns>A new instance of <see cref="IWindsorContainer"/>.</returns>
        protected virtual IWindsorContainer CreateContainer()
        {
            return new WindsorContainer();
        }

        /// <summary>
        /// Returns the module enumerator that will be used to initialize the modules.
        /// </summary>
        /// <remarks>
        /// When using the default initialization behavior, this method must be overwritten by a derived class.
        /// </remarks>
        /// <returns>An instance of <see cref="IModuleEnumerator"/> that will be used to initialize the modules.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected virtual IModuleEnumerator GetModuleEnumerator()
        {
            return null;
        }

        /// <summary>
        /// Registers a type in the container only if that type was not already registered.
        /// </summary>
        /// <param name="registerSingleton">registers the type as a singleton</param>
        protected void RegisterTypeIfMissing<TServiceType, TClassType>(bool registerSingleton) where TClassType : class, TServiceType
        {
            if (Container.IsTypeRegistered(typeof(TServiceType)))
            {
                LoggerFacade.Log(String.Format(CultureInfo.CurrentCulture, Resources.TypeMappingAlreadyRegistered, typeof(TServiceType).Name), Category.Debug, Priority.Low);
            }
            else
            {
                Container.Kernel.Register(
                    Component.For<TServiceType>()
                             .ImplementedBy<TClassType>()
                             .LifeStyle.Is(registerSingleton ? LifestyleType.Singleton : LifestyleType.Transient)
                    );
            }
        }

        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>The shell of the application.</returns>
        /// <remarks>
        /// If the returned instance is a <see cref="DependencyObject"/>, the
        /// <see cref="WindsorBootstrapper"/> will attach the default <seealso cref="IRegionManager"/> of
        /// the application in its <see cref="RegionManager.RegionManagerProperty"/> attached property
        /// in order to be able to add regions by using the <seealso cref="RegionManager.RegionNameProperty"/>
        /// attached property from XAML.
        /// </remarks>
        protected abstract DependencyObject CreateShell();

        #endregion
    }
}
