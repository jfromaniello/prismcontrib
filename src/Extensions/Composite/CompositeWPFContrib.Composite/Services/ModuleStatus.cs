namespace CompositeWPFContrib.Composite.Services
{
    /// <summary>
    /// Enumeration of the possible module statuses
    /// </summary>
    public enum ModuleStatus
    {
        /// <summary>
        /// The module has been loaded and is activated
        /// </summary>
        Loaded,

        /// <summary>
        /// The module was returned by the module enumerator, but is currently not loaded
        /// </summary>
        Unloaded
    }
}