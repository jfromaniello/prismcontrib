using System;
using DILight;
using Prism.Interfaces;
using PrismImageSearch.Infrastructure.Interfaces;

namespace PrismImageSearch.Modules.ResultBrowser
{
    public class ResultBrowserModule : IModule
    {
        #region Member Variables
        /************************************************
		 * Member Variables
		 ***********************************************/
        private IContainer container;
        private BrowserViewPresenter browserPresenter;
        #endregion // Member Variables

        #region Constructors
        /************************************************
		 * Constructors
		 ***********************************************/
        /// <summary>
        /// Initializes a new <see cref="ResultBrowserModule"/> instance.
        /// </summary>
        /// <param name="container">
        /// The container that will be used for DI resolution.
        /// </param>
        public ResultBrowserModule(IContainer container)
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
            container.RegisterType<IBrowserView, BrowserView>();

            // Create our browser presenter
            browserPresenter = container.Resolve<BrowserViewPresenter>();

            // Show the views
            browserPresenter.ShowViews();

            // Our browser presenter provides the selection service
            container.RegisterInstance<IResultSelectionService>(browserPresenter);
        }
        #endregion // Public Methods
    }
}
