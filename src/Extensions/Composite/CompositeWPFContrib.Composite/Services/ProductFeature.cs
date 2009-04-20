using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompositeWPFContrib.Composite.Services
{
    /// <summary>
    /// Contains information about a product feature
    /// </summary>
    public struct ProductFeature
    {
        /// <summary>
        /// The ID of the feature
        /// </summary>
        public string FeatureName;

        /// <summary>
        /// The ID of the parent feature
        /// </summary>
        public string ParentFeatureName;
    }
}
