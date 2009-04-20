using System.Windows;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Modularity;
using WPFQuickstartWithCAL.EmployeeData.Views;
using WPFQuickstartWithCAL.Infrastructure.Constants;

namespace WPFQuickstartWithCAL.EmployeeData
{
    public class EmployeeDataModule : IModule
    {
        private IUnityContainer container;
        private IRegionManager regionManager;

        public EmployeeDataModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            this.RegisterViewsAndServices();

            EmployeePresenter presenter = this.container.Resolve<EmployeePresenter>();

            IRegion mainRegion = this.regionManager.Regions[RegionNames.RightRegion];
            mainRegion.Add(presenter.View);
        }

        protected void RegisterViewsAndServices()
        {
            this.container.RegisterType<IEmployeeView, EmployeeView>();
        }
    }
}
