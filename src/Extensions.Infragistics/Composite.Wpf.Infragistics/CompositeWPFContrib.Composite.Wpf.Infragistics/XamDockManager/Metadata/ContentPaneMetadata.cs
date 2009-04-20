using System.Windows;

namespace CompositeWPFContrib.Composite.Wpf.Infragistics.DockManager.Metadata
{
    public class ContentPaneMetadata : DependencyObject, IContentPaneMetadata
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(ContentPaneMetadata));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
    }
}
