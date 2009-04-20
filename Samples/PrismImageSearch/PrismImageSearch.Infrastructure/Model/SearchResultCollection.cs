using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace PrismImageSearch.Infrastructure.Model
{
    public class SearchResultCollection : List<SearchResult>
    {
        public SearchResultCollection()
        {
            
        }

        public SearchResultCollection(IEnumerable<SearchResult> collection) : base(collection)
        {

        }
    }
}
