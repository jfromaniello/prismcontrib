using System;

namespace PrismImageSearch.Infrastructure.Model
{
    /// <summary>
    /// A single result for an image search.
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Gets or sets the title of the <see cref="ImageSearchResult"/>.
        /// </summary>
        /// <value>
        /// The title of the <c>ImageSearchResult</c>.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the date and time the image was taken.
        /// </summary>
        /// <value>
        /// The date and time the image was taken.
        /// </value>
        public DateTime DateTaken { get; set; }

        /// <summary>
        /// Gets or sets the description of the <see cref="ImageSearchResult"/>.
        /// </summary>
        /// <value>
        /// The description of the <c>ImageSearchResult</c>.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the location of the <see cref="ImageSearchResult"/>.
        /// </summary>
        /// <value>
        /// The location of the <c>ImageSearchResult</c>.
        /// </value>
        public string Location { get; set; }
    }
}
