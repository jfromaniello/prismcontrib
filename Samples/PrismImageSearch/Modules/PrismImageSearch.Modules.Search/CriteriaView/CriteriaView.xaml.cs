using System;
using System.Windows.Controls;
using PrismImageSearch.Infrastructure.Model;

namespace PrismImageSearch.Modules.Search
{
	public partial class CriteriaView : UserControl, ICriteriaView
	{
        #region Constructors
        /************************************************
		 * Constructors
		 ***********************************************/
        /// <summary>
        /// Initializes a new <see cref="CriteriaView"/> instance.
        /// </summary>
        public CriteriaView()
        {
            // Required to initialize variables
            InitializeComponent();
        }
        #endregion // Constructors

        #region Overrides / Event Handlers
        /************************************************
		 * Overrides / Event Handlers
		 ***********************************************/
        private void SearchButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (SearchRequested != null)
            {
                SearchRequested(this, e);
            }
        }
        #endregion // Overrides / Event Handlers

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
        public string SearchText
        {
            get
            {
                return SearchCriteria.Text;
            }
        }
        #endregion // Public Properties

        #region Public Events
        /************************************************
		 * Public Events
		 ***********************************************/
        /// <summary>
        /// Occurs when a search is being requested through the view.
        /// </summary>
        public event EventHandler SearchRequested;
        #endregion // Public Events
    }
}