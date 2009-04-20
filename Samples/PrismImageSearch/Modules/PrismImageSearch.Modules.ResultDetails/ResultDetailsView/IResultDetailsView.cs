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

namespace PrismImageSearch.Modules.ResultDetails
{
    /// <summary>
    /// The interface for a result browser view.
    /// </summary>
    public interface IResultDetailsView
    {

        /// <summary>
        /// Gets or sets the search result that is currently displayed.
        /// </summary>
        /// <value>
        /// The search result that is currently displayed.
        /// </value>
        SearchResult Result { get; set; }
    }
}
