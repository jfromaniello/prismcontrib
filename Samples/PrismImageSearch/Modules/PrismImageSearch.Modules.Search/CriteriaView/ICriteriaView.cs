using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PrismImageSearch.Infrastructure.Interfaces;
using PrismImageSearch.Infrastructure.Model;

namespace PrismImageSearch.Modules.Search
{
    /// <summary>
    /// The interface for a search criteria view.
    /// </summary>
    public interface ICriteriaView
    {
        #region Public Properties
        /************************************************
		 * Public Properties
		 ***********************************************/
        /// <summary>
        /// Gets the text to be searched on.
        /// </summary>
        /// <value>
        /// The text to be searched on.
        /// </value>
        string SearchText { get; }
        #endregion // Public Properties

        #region Public Events
        /************************************************
		 * Public Events
		 ***********************************************/
        /// <summary>
        /// Occurs when a search is being requested through the view.
        /// </summary>
        event EventHandler SearchRequested;
        #endregion // Public Events
    }
}
