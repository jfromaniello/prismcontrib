using System;
using DILight;
using Prism.Interfaces;
using PrismImageSearch.Infrastructure.Interfaces;

namespace PrismImageSearch.Modules.ResultDetails
{
    public class ResultDetailsModule : IModule
    {
        #region Member Variables
        /************************************************
		 * Member Variables
		 ***********************************************/
        private IContainer container;
        private ResultDetailsViewPresenter detailsPresenter;
        #endregion // Member Variables

        #region Constructors
        /************************************************
		 * Constructors
		 ***********************************************/
        /// <summary>
        /// Initializes a new <see cref="ResultDetailsModule"/> instance.
        /// </summary>
        /// <param name="container">
        /// The container that will be used for DI resolution.
        /// </param>
        public ResultDetailsModule(IContainer container)
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
            // Register views and services with the container
            RegisterViewsAndServices();
        }

        public void RegisterViewsAndServices()
        {
            // Setup view mapping
            container.RegisterType<IResultDetailsView, ResultDetailsView>();

            // Create our browser presenter
            detailsPresenter = container.Resolve<ResultDetailsViewPresenter>();

            // Show the views
            detailsPresenter.ShowViews();
        }
        #endregion // Public Methods
    }
}
