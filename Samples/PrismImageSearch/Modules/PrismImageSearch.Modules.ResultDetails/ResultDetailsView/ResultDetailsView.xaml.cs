using System;
using System.Windows.Controls;
using PrismImageSearch.Infrastructure.Model;

namespace PrismImageSearch.Modules.ResultDetails
{
	public partial class ResultDetailsView : UserControl, IResultDetailsView
	{

        public ResultDetailsView()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        #region IResultDetailsView Members
        public SearchResult Result
        {
            get
            {
                return (SearchResult)DataContext;
            }
            set
            {
                DataContext = value;
            }
        }
        #endregion
    }
}