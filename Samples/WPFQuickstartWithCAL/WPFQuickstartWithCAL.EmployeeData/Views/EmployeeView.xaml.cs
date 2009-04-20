using System.Windows.Controls;
using WPFQuickstartWithCAL.EmployeeData.BusinessEntities;

namespace WPFQuickstartWithCAL.EmployeeData.Views
{
    /// <summary>
    /// Interaction logic for EmployeeView.xaml
    /// </summary>
    public partial class EmployeeView : UserControl, IEmployeeView
    {
        public EmployeeView()
        {
            InitializeComponent();
        }

        public Employee Model
        {
            get { return this.DataContext as Employee; }
            set { this.DataContext = value; }
        }
    }
}
