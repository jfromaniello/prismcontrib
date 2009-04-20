using System.Windows;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using WPFQuickstartWithCAL.OrganizationChart.Views;
using WPFQuickstartWithCAL.Infrastructure.Constants;

namespace WPFQuickstartWithCAL.OrganizationChart
{
    public class OrganizationChartModule : IModule
    {
        private IUnityContainer container;
        private IRegionManager regionManager;

        public OrganizationChartModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            this.RegisterViewsAndServices();

            OrgChartPresenter presenter = this.container.Resolve<OrgChartPresenter>();

            IRegion mainRegion = this.regionManager.Regions[RegionNames.LeftRegion];
            mainRegion.Add(presenter.View);
        }

        protected void RegisterViewsAndServices()
        {
            this.container.RegisterType<IOrgChartView, OrgChartView>();
        }
    }
}
