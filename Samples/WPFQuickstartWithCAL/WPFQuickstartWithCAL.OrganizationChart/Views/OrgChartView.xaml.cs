using System;
using System.Xml;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using Microsoft.Practices.Composite.Events;

namespace WPFQuickstartWithCAL.OrganizationChart.Views
{
    /// <summary>
    /// Interaction logic for OrgChartView.xaml
    /// </summary>
    public partial class OrgChartView : UserControl, IOrgChartView
    {
        public OrgChartView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty OrgChartDataSourceProperty =
            DependencyProperty.Register("OrgChartDataSource", typeof(XmlDocument), typeof(OrgChartView), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnValueChanged)));

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            OrgChartView control = (OrgChartView)obj;
            XmlDataProvider provider = (XmlDataProvider)control.Resources["xdpOrgChart"];
            provider.Document = (XmlDocument)args.NewValue;
        }

        void SelectionChanged(object sender, RoutedEventArgs e)
        {
            XmlElement element = ((XmlElement)((TreeView)e.Source).SelectedItem);
            PositionSelected(this, new DataEventArgs<XmlElement>(element));
        }

        public XmlDocument Model
        {
            get { return (XmlDocument)GetValue(OrgChartDataSourceProperty); }
            set { SetValue(OrgChartDataSourceProperty, value); }
        }

        public event EventHandler<DataEventArgs<XmlElement>> PositionSelected = delegate { };
    }
}
