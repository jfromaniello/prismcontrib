using AvalonDockRegionAdapterSample.Views;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;

namespace AvalonDockRegionAdapterSample
{
    /// <summary>
    /// created 18.02.2009 by Markus Raufer
    /// </summary>
    public class MainModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public MainModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            IRegion leftRegion = _regionManager.Regions[RegionNames.LeftRegion];
            var view1 = new View1();
            leftRegion.Add( view1 );

            IRegion documentRegion = _regionManager.Regions[RegionNames.DocumentRegion];
            var view2 = new View2();
            documentRegion.Add(view2);

            IRegion bottomRegion = _regionManager.Regions[RegionNames.BottomRegion];
            var view3 = new View3();
            bottomRegion.Add(view3);
        }
    }
}
