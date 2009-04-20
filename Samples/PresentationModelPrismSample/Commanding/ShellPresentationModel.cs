using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Regions;

namespace Commanding
{
    public class ShellPresentationModel
    {
        public ShellPresentationModel(IRegionManager regionManager)
        {
            MainRegion = new Region();
            GlobalCommandsRegion = new Region();
            regionManager.Regions.Add("MainRegion", MainRegion);
            regionManager.Regions.Add("GlobalCommandsRegion", GlobalCommandsRegion);
        }

        public IRegion MainRegion { get; private set; }
        public IRegion GlobalCommandsRegion { get; private set; }
    }
}
