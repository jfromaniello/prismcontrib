using System;
using System.Windows;
using Prism.Interfaces;
using PrismImageSearch.Infrastructure;
using PrismImageSearch.Infrastructure.Interfaces;

namespace PrismImageSearch.Modules.ResultDetails
{
    /// <summary>
    /// The presenter for a <see cref="BrowserView"/>.
    /// </summary>
    public class ResultDetailsViewPresenter
    {
        #region Member Variables
        /************************************************
		 * Member Variables
		 ***********************************************/
        private IResultDetailsView view;
        private IRegionManagerService regionManager;
        private IResultSelectionService resultSelector;
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
        public ResultDetailsViewPresenter(IResultDetailsView view, IRegionManagerService regionManager, IResultSelectionService resultSelector)
        {
            // Store locally
            this.view = view;
            this.regionManager = regionManager;
            this.resultSelector = resultSelector;

            // Subscribe to selection change notifications
            resultSelector.SelectedResultChanged += new EventHandler(resultSelector_SelectedResultChanged);
        }
        #endregion // Constructors

        #region Overrides / Event Handlers
        /************************************************
		 * Overrides / Event Handlers
		 ***********************************************/
        private void resultSelector_SelectedResultChanged(object sender, EventArgs e)
        {
            // Update the view
            view.Result = resultSelector.SelectedResult;
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
            IRegion detailRegion = regionManager.GetRegion(RegionNames.Detail);

            // Add the view
            detailRegion.Add((UIElement)view);
        }
        #endregion // Public Methods

        #region Public Properties
        /************************************************
		 * Public Properties
		 ***********************************************/
        /// <summary>
        /// Gets the result details view of the <see cref="ResultDetailsViewPresenter"/>.
        /// </summary>
        /// <value>
        /// The rsult details view of the <c>ResultDetailsViewPresenter</c>.
        /// </value>
        public IResultDetailsView ResultDetailsView
        {
            get
            {
                return view;
            }
        }
        #endregion // Public Properties
    }
}
