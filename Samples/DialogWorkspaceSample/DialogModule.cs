using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Unity;
using CompositeWPFContrib.Composite.Wpf;
using System.Windows;
using System.Globalization;

namespace DialogWorkspaceSample
{
    /// <summary>
    /// Module to demonstrate the dialog functionality
    /// </summary>
    public class DialogModule: IModule
    {
        private IUnityContainer _container;

        /// <summary>
        /// Initializes a new instance of the DialogModule class.
        /// </summary>
        /// <param name="container"></param>
        public DialogModule(IUnityContainer container)
        {
            _container = container;
            _container.RegisterType<ISampleView, SampleView>();
        }

        #region IModule Members

        /// <summary>
        /// Initializes the module
        /// </summary>
        public void Initialize()
        {
            ISampleView view = _container.Resolve<ISampleView>();
            bool? result = DialogWorkspace.ShowView(view,() => 
            {
                if (view.FirstName.Length == 0 || view.LastName.Length == 0)
                {
                    MessageBox.Show("Not all required fields are present", 
                        "Validation failed", MessageBoxButton.OK, MessageBoxImage.Error);

                    return false;
                }

                return true;
            });

            if (result == true)
            {
                MessageBox.Show(String.Format(CultureInfo.InvariantCulture, "Hello {0} {1}", 
                    view.FirstName, view.LastName),
                    "Hello there", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion
    }
}
