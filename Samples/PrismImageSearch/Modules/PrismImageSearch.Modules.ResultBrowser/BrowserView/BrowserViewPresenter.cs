using System;
using System.Windows;
using Prism.Interfaces;
using PrismImageSearch.Infrastructure;
using PrismImageSearch.Infrastructure.Interfaces;
using PrismImageSearch.Infrastructure.Model;

namespace PrismImageSearch.Modules.ResultBrowser
{
    /// <summary>
    /// The presenter for a <see cref="BrowserView"/>.
    /// </summary>
    public class BrowserViewPresenter : IResultSelectionService
    {
        #region Member Variables
        /************************************************
		 * Member Variables
		 ***********************************************/
        private IBrowserView view;
        private IRegionManagerService regionManager;
        private ISearchEngine searchEngine;
        #endregion // Member Variables

        #region Constructors
        /************************************************
		 * Constructors
		 ***********************************************/
        /// <summary>
        /// Initializes a new <see cref="BrowserViewPresenter"/> instance.
        /// </summary>
        /// <param name="view">
        /// The view the presenter will control.
        /// </param>
        /// <param name="regionManager">
        /// The region manager that can be used to place views.
        /// </param>
        /// <param name="searchEngine">
        /// The search engine that will provide results.
        /// </param>
        public BrowserViewPresenter(IBrowserView view, IRegionManagerService regionManager, ISearchEngine searchEngine)
        {
            // Store locally
            this.view = view;
            this.regionManager = regionManager;
            this.searchEngine = searchEngine;

            // Subscribe to selection events from the view to bubble up
            view.SelectedResultChanged += new EventHandler(view_SelectedResultChanged);

            // Subscribe to search result notifications
            searchEngine.SearchComplete += new EventHandler<SearchCompleteEventArgs>(searchEngine_SearchComplete);
        }
        #endregion // Constructors

        #region Overrides / Event Handlers
        /************************************************
		 * Overrides / Event Handlers
		 ***********************************************/
        private void view_SelectedResultChanged(object sender, EventArgs e)
        {
            // If subscribers exist, just bubble.
            if (SelectedResultChanged != null)
            {
                SelectedResultChanged(this, e);
            }
        }

        private void searchEngine_SearchComplete(object sender, SearchCompleteEventArgs e)
        {
            // Show results in the view
            BrowserView.SetResults(e.Results);
        }
        #endregion // Overrides / Event Handlers

        #region Public Methods
        /************************************************
		 * Public Methods
		 ***********************************************/
        /// <summary>
        /// Shows our views on the screen.
        /// </summary>
        public void ShowViews()
        {
            // Get main region
            IRegion mainRegion = regionManager.GetRegion(RegionNames.Master);
            
            // Add the view
            mainRegion.Add((UIElement)view);
        }
        #endregion // Public Methods

        #region Public Properties
        /************************************************
		 * Public Properties
		 ***********************************************/
        /// <summary>
        /// Gets the browser view of the <see cref="BrowserViewPresenter"/>.
        /// </summary>
        /// <value>
        /// The browser view of the <c>BrowserViewPresenter</c>.
        /// </value>
        public IBrowserView BrowserView
        {
            get
            {
                return view;
            }
        }

        /// <summary>
        /// Gets or sets the selected search result.
        /// </summary>
        /// <value>
        /// The selected search result.
        /// </value>
        public SearchResult SelectedResult
        {
            get
            {
                // Pull from our view
                return view.SelectedResult;
            }
            set
            {
                // Push into the view
                view.SelectedResult = value;
            }
        }
        #endregion // Public Properties

        #region Public Events
        /************************************************
		 * Public Events
		 ***********************************************/
        /// <summary>
        /// Occurs whenever the value of the <see cref="SelectedResult"/> property has changed.
        /// </summary>
        public event EventHandler SelectedResultChanged;
        #endregion // Public Events
    }
}
