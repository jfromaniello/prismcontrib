using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Composite.Modularity;

namespace CompositeWPFContrib.Composite.Services
{
    /// <summary>
    /// Implementation of the extended module loader service.
    /// 
    /// Usage: Set the ProductCode property of this service to the product code
    /// specified in the MSI package used to install the application.
    /// </summary>
    public class ExtendedModuleLoaderService: IExtendedModuleLoaderService
    {
        private IUnityContainer _container;
        private string _productCode;

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the ExtendedModuleLoaderService class.
        /// </summary>
        /// <param name="container"></param>
        public ExtendedModuleLoaderService(IUnityContainer container)
        {
            _container = container;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the Windows Installer Product Code
        /// to use when checking if a particular module is installed
        /// </summary>
        public string ProductCode
        {
            get
            {
                return _productCode;
            }
            set
            {
                if (_productCode == value)
                    return;
                _productCode = value;
            }
        }
        
        #endregion

        #region IExtendedModuleLoaderService Members

        /// <summary>
        /// Loads a single module and installs
        /// it if it is not installed
        /// </summary>
        /// <param name="moduleName"></param>
        public void LoadModule(string moduleName)
        {
            if (String.IsNullOrEmpty(moduleName))
                throw new ArgumentNullException("moduleName");

            LoadModule(moduleName, true);
        }

        /// <summary>
        /// Loads a single module and installs it if it is not
        /// installed and the installIfNotAvailable parameter is set to true
        /// </summary>
        /// <param name="moduleName">Name of the module</param>
        /// <param name="installIfNotAvailable">Whether to install the module if available</param>
        public void LoadModule(string moduleName, bool installIfNotAvailable)
        {
            if(String.IsNullOrEmpty(moduleName))
                throw new ArgumentNullException("moduleName");
            
            IModuleCatalog moduleEnumerator = _container.Resolve<IModuleCatalog>();
            ModuleLoader moduleLoader = _container.Resolve<ModuleLoader>();

            // Retrieve the required module information
            ModuleInfo module = moduleEnumerator.GetModule(moduleName);

            if (module == null)
            {
                throw new ArgumentException("Request module was not found",
                    "moduleName");
            }

            // Check if the module should be installed
            if (installIfNotAvailable)
            {
                InstallState installationState = 
                    WindowsInstaller.GetFeatureState(_productCode,moduleName);

                if (installationState != InstallState.Local && 
                    installationState != InstallState.Default)
                {
                    // Make sure that the feature is installed locally
                    WindowsInstaller.ConfigureFeature(
                        _productCode, moduleName, InstallState.Local);
                }
            }

            // Initialize the module
            moduleLoader.Initialize(new ModuleInfo[] { module });
        }

        #endregion
    }
}
