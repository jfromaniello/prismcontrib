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

namespace PrismImageSearch.Modules.ResultBrowser
{
    /// <summary>
    /// The interface for a result browser view.
    /// </summary>
    public interface IBrowserView : IResultSelectionService
    {
        /// <summary>
        /// Sets the list of active search results to be browsed.
        /// </summary>
        /// <param name="results">
        /// The list of results to be browsed.
        /// </param>
        void SetResults(SearchResultCollection results);
    }
}
