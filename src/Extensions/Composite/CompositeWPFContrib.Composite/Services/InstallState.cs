using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompositeWPFContrib.Composite.Services
{
    /// <summary>
    /// Enumeration of the possible Windows Installer Installation States
    /// </summary>
    public enum InstallState
    {
        /// <summary>
        /// Feature is not being used
        /// </summary>
        NotUsed = -7,

        /// <summary>
        /// Feature has bad configuration data
        /// </summary>
        BadConfig = -6,

        /// <summary>
        /// Feature installation is incomplete
        /// </summary>
        Incomplete = -5,

        /// <summary>
        /// The source for the feature is unavailable
        /// </summary>
        SourceAbsent = -4,

        /// <summary>
        /// Feature is not advertised and not installed
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// Feature is broken
        /// </summary>
        Broken = 0,

        /// <summary>
        /// Feature is advertised but not installed
        /// </summary>
        Advertised = 1,

        /// <summary>
        /// Feature is installed for different user
        /// </summary>
        Absent = 2,

        /// <summary>
        /// Feature is installed locally
        /// </summary>
        Local = 3,

        /// <summary>
        /// Feature is being run from source
        /// </summary>
        Source = 4,

        /// <summary>
        /// Feature is installed for current user
        /// </summary>
        Default = 5
    }
}
