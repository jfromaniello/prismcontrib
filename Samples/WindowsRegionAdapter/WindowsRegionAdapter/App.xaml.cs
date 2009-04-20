using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using WRAExample;

namespace WRAExample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}
