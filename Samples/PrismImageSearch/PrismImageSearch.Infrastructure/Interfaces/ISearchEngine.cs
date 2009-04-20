using System;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Collections.Generic;
using PrismImageSearch.Infrastructure.Model;

namespace PrismImageSearch.Infrastructure.Interfaces
{
    #region Event Args
    public class SearchCompleteEventArgs : EventArgs
    {
        /// <summary>
        /// The constructor for a successful search.
        /// </summary>
        /// <param name="results">
        /// The collection of search results.
        /// </param>
        public SearchCompleteEventArgs(SearchResultCollection results)
        {
            // Validate
            if (results == null) throw new ArgumentNullException("results");

            // Store
            Results = results;
        }

        /// <summary>
        /// The constructor for a failed search.
        /// </summary>
        /// <param name="error">
        /// The exception that caused the search to fail.
        /// </param>
        public SearchCompleteEventArgs(Exception error)
        {
            // Validate
            if (error == null) throw new ArgumentNullException("error");

            // Store
            Error = error;
        }

        /// <summary>
        /// Gets the exception that cauesd the search to fail.
        /// </summary>
        /// <value>
        /// The exception that cauesd the search to fail.
        /// </value>
        public Exception Error { get; private set; }
        
        /// <summary>
        /// Gets the results of the <see cref="SearchResultEventArgs"/>.
        /// </summary>
        /// <value>
        /// The results of the <c>SearchResultEventArgs</c>.
        /// </value>
        public SearchResultCollection Results { get; private set; }
    }
    #endregion // Event Args

    /// <summary>
    /// The interface for an image search engine service.
    /// </summary>
    public interface ISearchEngine
    {
        #region Public Methods
        /************************************************
		 * Public Methods
		 ***********************************************/
        /// <summary>
        /// Searches for images with the specified tag.
        /// </summary>
        /// <param name="tag">
        /// The tag to search for.
        /// </param>
        void Search(string tag);
        #endregion // Public Methods

        #region Public Events
        /************************************************
		 * Public Events
		 ***********************************************/
        /// <summary>
        /// Occurs when a search is completed.
        /// </summary>
        event EventHandler<SearchCompleteEventArgs> SearchComplete;
        #endregion // Public Events        
    }
}
