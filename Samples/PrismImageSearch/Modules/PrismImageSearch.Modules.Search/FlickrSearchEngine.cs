using System;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using PrismImageSearch.Infrastructure.Interfaces;
using PrismImageSearch.Infrastructure.Model;

namespace PrismImageSearch.Modules.Search
{
    /// <summary>
    /// An image search engine.
    /// </summary>
    public class FlickrSearchEngine : ISearchEngine
    {
        #region Helper Methods
        /************************************************
		 * Helper Methods
		 ***********************************************/
        static private string MakeFlickrUrl(string farmId, string serverId, string imageId, string secret)
        {
            return string.Format("http://farm{0}.static.flickr.com/{1}/{2}_{3}.jpg",
                farmId,
                serverId,
                imageId,
                secret);
        }

        static private string MakeFlickrUrl(XElement element)
        {
            return MakeFlickrUrl(
                (string)element.Attribute("farm"),
                (string)element.Attribute("server"),
                (string)element.Attribute("id"),
                (string)element.Attribute("secret")
                );
        }

        private void NotifyResult(SearchCompleteEventArgs e)
        {
            if (SearchComplete != null)
            {
                SearchComplete(this, e);
            }
        }
        #endregion // Helper Methods

        #region Internal Methods
        /************************************************
		 * Internal Methods
		 ***********************************************/
        /// <summary>
        /// Searches the Flickr site.
        /// </summary>
        /// <param name="tag">
        /// The tag to search for.
        /// </param>
        private void SearchFlickr(string tag)
        {
            // Without tags
            string tagSearchUrl = "http://api.flickr.com/services/rest/?api_key=b66e14b24ff0f0ceec36eb86e1e15678&method=flickr.photos.search&tags={0}&per_page=10&page=1&extras=date_taken";
            
            // Create search Uri from tag search
            Uri searchUri = new Uri(string.Format(tagSearchUrl, tag));

            // We'll use WebClient to make the request asynchronously
            WebClient client = new WebClient();

            // Prepare to handle results asynchronously
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(HandleFlickrResult);

            // Request results asynchronously
            client.DownloadStringAsync(searchUri);
        }

        private void HandleFlickrResult(object sender, DownloadStringCompletedEventArgs e)
        {
            // Different branch for success than failure
            if (e.Error == null)
            {
                // Get XML from the result string
                XDocument xmlResults = XDocument.Parse(e.Result);

                // Process reults using LINQ to XML
                var enumResults = from result in xmlResults.Descendants("photo")
                              select new SearchResult
                              {
                                   DateTaken = (DateTime)result.Attribute("datetaken"),
                                   // Description = ?,
                                   Location = MakeFlickrUrl(result),
                                   Title = (string)result.Attribute("title"),
                              };

                // Create a new collection containing all results
                SearchResultCollection results = new SearchResultCollection(enumResults);

                // Notify of results
                NotifyResult(new SearchCompleteEventArgs(results));
            }
            else
            {
                // Failure, notify
                NotifyResult(new SearchCompleteEventArgs(e.Error));
            }
        }
        #endregion // Internal Methods

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
        public void Search(string tag)
        {
            // Only proceed if we've got valid search criteria.
            if (!string.IsNullOrEmpty(tag))
            {
                // Just call internal Flickr search method
                SearchFlickr(tag);

                // TODO: Could rename this class and add other search engines here.
            }
        }
        #endregion // Public Methods

        #region Public Events
        /************************************************
		 * Public Events
		 ***********************************************/
        /// <summary>
        /// Occurs when a search is completed.
        /// </summary>
        public event EventHandler<SearchCompleteEventArgs> SearchComplete;
        #endregion // Public Events        
    }
}
