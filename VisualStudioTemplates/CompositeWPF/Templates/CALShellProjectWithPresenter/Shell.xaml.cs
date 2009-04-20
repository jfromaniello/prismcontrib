using System.Windows;

namespace $safeprojectname$
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window, IShellView
    {
        public Shell()
        {
            InitializeComponent();
        }

        public void ShowView()
        {
            this.Show();
        }
    }
}
