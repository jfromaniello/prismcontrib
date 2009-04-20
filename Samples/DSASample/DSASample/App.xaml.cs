using System.Windows;

namespace DSASample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Bootstrapper bootStrapper = new Bootstrapper();
            bootStrapper.Run();
        }
    }
}