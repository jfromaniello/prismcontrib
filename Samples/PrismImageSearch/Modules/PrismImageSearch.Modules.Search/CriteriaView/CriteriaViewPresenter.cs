using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Prism.Interfaces;
using PrismImageSearch.Infrastructure.Interfaces;
using PrismImageSearch.Infrastructure.Model;
using PrismImageSearch.Infrastructure;

namespace PrismImageSearch.Modules.Search
{
    /// <summary>
    /// The presenter for a <see cref="CriteriaView"/>.
    /// </summary>
    public class CriteriaViewPresenter
    {
        #region Member Variables
        /************************************************
		 * Member Variables
		 ***********************************************/
        private ICriteriaView view;
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
        public CriteriaViewPresenter(ICriteriaView view, IRegionManagerService regionManager, ISearchEngine searchEngine)
        {
            // Store locally
            this.view = view;
            this.regionManager = regionManager;
            this.searchEngine = searchEngine;

            // Subscribe to events from the view
            view.SearchRequested += new EventHandler(view_SearchRequested);
        }
        #endregion // Constructors

        #region Overrides / Event Handlers
        /************************************************
		 * Overrides / Event Handlers
		 ***********************************************/
        private void view_SearchRequested(object sender, EventArgs e)
        {
            searchEngine.Search(view.SearchText);
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
            IRegion criteriaRegion = regionManager.GetRegion(RegionNames.Criteria);

            // Add the view
            criteriaRegion.Add((UIElement)view);
        }
        #endregion // Public Methods

        #region Public Properties
        /************************************************
		 * Public Properties
		 ***********************************************/
        /// <summary>
        /// Gets the criteria view of the <see cref="CriteriaViewPresenter"/>.
        /// </summary>
        /// <value>
        /// The criteria view of the <c>CriteriaViewPresenter</c>.
        /// </value>
        public ICriteriaView CriteriaView
        {
            get
            {
                return view;
            }
        }
        #endregion // Public Properties
    }
}
