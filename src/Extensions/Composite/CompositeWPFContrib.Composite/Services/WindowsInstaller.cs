using System;
using System.Runtime.InteropServices;

namespace CompositeWPFContrib.Composite.Services
{
    /// <summary>
    /// Wrapper class for Windows Installer Functionality
    /// </summary>
    public static class WindowsInstaller
    {
        /// <summary>
        /// Retrieves the install state of a specific feature
        /// </summary>
        /// <param name="productCode">Product code of the product</param>
        /// <param name="featureName">Name of the feature</param>
        /// <returns>Returns the install state of the product</returns>
        [DllImport("msi.dll",EntryPoint="MsiQueryFeatureState",CharSet=CharSet.Unicode)]
        public extern static InstallState GetFeatureState(string productCode, string featureName);

        /// <summary>
        /// Configures a feature of the specified application
        /// </summary>
        /// <param name="productCode">Product code of the application</param>
        /// <param name="featureName">Name of the feature to configure</param>
        /// <param name="state">Install state to configure</param>
        /// <remarks>
        /// The following install states can be configured:
        /// <see cref="InstallState.Local"/> Feature will be installed locally
        /// <see cref="InstallState.Absent"/> Feature will be uninstalled
        /// <see cref="InstallState.Source"/> Feature will run from source
        /// <see cref="InstallState.Default"/> Feature will be installed to default location
        /// </remarks>
        /// <returns></returns>
        [DllImport("msi.dll", EntryPoint = "MsiConfigureFeature", CharSet = CharSet.Unicode)]
        public extern static uint ConfigureFeature(string productCode,
            string featureName,
            InstallState state);

        /// <summary>
        /// Contains the Windows installer error codes
        /// </summary>
        public static class ErrorCodes
        {
            /// <summary>
            /// Indicates that the operation succeeded
            /// </summary>
            public const uint Success = 0;

            /// <summary>
            /// Indicates that an invalid or non-existing product code
            /// was passed to the method
            /// </summary>
            public const uint InvalidProductCode = 1605;

            /// <summary>
            /// Indicates that there are no more items available in
            /// the enumeration. This is not an error
            /// </summary>
            public const uint NoMoreItemsInEnumeration = 259;

            /// <summary>
            /// Indicates that the size of the input buffer is too small
            /// </summary>
            public const uint BufferSizeTooSmall = 234;
        }
    }
}
