using System;
using DILight;
using Prism.Interfaces;
using PrismImageSearch.Infrastructure.Interfaces;

namespace PrismImageSearch.Modules.Search
{
    public class SearchModule : IModule
    {
        #region Member Variables
        /************************************************
		 * Member Variables
		 ***********************************************/
        private IContainer container;
        private CriteriaViewPresenter criteriaPresenter; 
        #endregion // Member Variables

        #region Constructors
        /************************************************
		 * Constructors
		 ***********************************************/
        /// <summary>
        /// Initializes a new <see cref="SearchModule"/> instance.
        /// </summary>
        /// <param name="container">
        /// The container that will be used for DI resolution.
        /// </param>
        public SearchModule(IContainer container)
        {
            // Validate
            if (container == null) throw new ArgumentNullException("container");

            // Store
            this.container = container;
        }
        #endregion // Constructors

        #region Public Methods
        /************************************************
		 * Public Methods
		 ***********************************************/
        public void Initialize()
        {
            // Just register views and services
            RegisterViewsAndServices();
        }

        public void RegisterViewsAndServices()
        {
            // Just adding one service, which searches Flickr
            container.RegisterInstance<ISearchEngine>(new FlickrSearchEngine());

            // Setup view mapping
            container.RegisterType<ICriteriaView, CriteriaView>();

            // Create our browser presenter
            criteriaPresenter = container.Resolve<CriteriaViewPresenter>();

            // Show the views
            criteriaPresenter.ShowViews();
        }
        #endregion // Public Methods
    }
}
