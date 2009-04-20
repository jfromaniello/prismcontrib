using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using System.Windows.Controls;
using WRAExample.MyModule;

namespace WRAExample.MyModule
{
    public class MyModuleInit : IModule
    {
        private IRegionManager _regionManager;
        private IUnityContainer _container;

        public MyModuleInit(IRegionManager regionManager, IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        #region IModule Members

        public void Initialize()
        {
            Button button = new Button();
            button.Content = "New View";
            button.Click += new System.Windows.RoutedEventHandler(button_Click);

            _regionManager.Regions["MainRegion"].Add(button);
        }

        void button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            View1 view = new View1();
            view.Title = "Some Title";

            _regionManager.Regions["MyWindowRegion"].Add(view);
        }

        #endregion
    }
}
