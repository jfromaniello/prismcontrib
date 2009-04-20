using System;
using System.Runtime.Serialization;

namespace CompositeWPFContrib.Composite.Services
{
    /// <summary>
    /// Indicates a problem with a Windows Installer operation
    /// </summary>
    [Serializable]
    public class WindowsInstallerException : Exception
    {
        #region WindowsInstallerException()
        /// <summary>
        /// Constructs a new WindowsInstallerException.
        /// </summary>
        public WindowsInstallerException() { }
        #endregion
        #region WindowsInstallerException(string message)
        /// <summary>
        /// Constructs a new WindowsInstallerException.
        /// </summary>
        /// <param name="message">The exception message</param>
        public WindowsInstallerException(string message) : base(message) { }
        #endregion
        #region WindowsInstallerException(string message, Exception innerException)
        /// <summary>
        /// Constructs a new WindowsInstallerException.
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerException">The inner exception</param>
        public WindowsInstallerException(string message, Exception innerException) : base(message, innerException) { }
        #endregion
        #region WindowsInstallerException(SerializationInfo info, StreamingContext context)
        /// <summary>
        /// Serialization constructor.
        /// </summary>
        protected WindowsInstallerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        #endregion
    }
}
