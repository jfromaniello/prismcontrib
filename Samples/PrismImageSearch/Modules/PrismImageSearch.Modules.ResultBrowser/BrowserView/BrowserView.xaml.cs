using System;
using System.Windows.Controls;
using PrismImageSearch.Infrastructure.Model;

namespace PrismImageSearch.Modules.ResultBrowser
{
	public partial class BrowserView : UserControl, IBrowserView
	{

        public BrowserView()
		{
			// Required to initialize variables
			InitializeComponent();

		}

        private void SearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If subscribers are available, notify
            if (SelectedResultChanged != null)
            {
                SelectedResultChanged(this, e);
            }
        }

        #region IBrowserView Members

        public void SetResults(SearchResultCollection results)
        {
            // Just set it to our entire contols data context.
            DataContext = results;
        }

        #endregion

        #region IResultSelectionService Members

        public SearchResult SelectedResult
        {
            get
            {
                return (SearchResult)SearchResults.SelectedItem;
            }
            set
            {
                SearchResults.SelectedItem = value;
            }
        }

        public event EventHandler SelectedResultChanged;

        #endregion
    }
}