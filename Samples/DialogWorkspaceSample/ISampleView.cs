using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DialogWorkspaceSample
{
    /// <summary>
    /// Contract for the sample view
    /// </summary>
    public interface ISampleView
    {
        /// <summary>
        /// Gets the first name that was provided
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// Gets the last name that was provided
        /// </summary>
        string LastName { get; set; }
    }
}
