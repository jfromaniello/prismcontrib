namespace CompositeWPFContrib.Composite.Services
{
    /// <summary>
    /// Contract for the extended module loader service
    /// </summary>
    public interface IExtendedModuleLoaderService
    {
        /// <summary>
        /// Loads a single module and installs
        /// it if it is not installed
        /// </summary>
        /// <param name="moduleName"></param>
        void LoadModule(string moduleName);

        /// <summary>
        /// Loads a single module and installs it if it is not 
        /// installed and the installIfNotAvailable parameter is set to true
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="installOfNotAvailable"></param>
        void LoadModule(string moduleName, bool installOfNotAvailable);

        /// <summary>
        /// Gets or sets the Windows Installer Product Code
        /// to use when checking if a particular module is installed
        /// </summary>
        string ProductCode { get; set; }
    }
}
