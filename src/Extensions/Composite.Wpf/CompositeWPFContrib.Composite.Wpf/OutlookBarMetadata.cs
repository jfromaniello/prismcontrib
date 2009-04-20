using System.Windows;
using System.Windows.Media;

namespace CompositeWPFContrib.Composite.Wpf
{
    public class OutlookBarMetadata : DependencyObject, IOutlookBarMetadata
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(OutlookBarMetadata));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(OutlookBarMetadata));

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public string Payload { get; set; }
    }
}