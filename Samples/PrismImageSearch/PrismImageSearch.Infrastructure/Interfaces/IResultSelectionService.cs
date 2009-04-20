using System;
using PrismImageSearch.Infrastructure.Model;

namespace PrismImageSearch.Infrastructure.Interfaces
{
    /// <summary>
    /// A service that allows the selection of one result and notifies when the selection has changed.
    /// </summary>
    public interface IResultSelectionService
    {
        /// <summary>
        /// Gets or sets the selected search result.
        /// </summary>
        /// <value>
        /// The selected search result.
        /// </value>
        SearchResult SelectedResult { get; set; }

        /// <summary>
        /// Occurs whenever the value of the <see cref="SelectedResult"/> property has changed.
        /// </summary>
        event EventHandler SelectedResultChanged;
    }
}
