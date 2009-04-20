using System.Windows;
using System.Windows.Controls;

namespace CompositeWPFContrib.Composite.Wpf.Controls
{
    public class OutlookBarControl : TabControl
    {
        public OutlookBarControl()
        {
            PropertyMetadata propertyMetadata = DefaultStyleKeyProperty.GetMetadata(typeof (OutlookBarControl));
            if ((propertyMetadata == null) || (propertyMetadata.DefaultValue != typeof(OutlookBarControl)))
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(OutlookBarControl),
                                                         new FrameworkPropertyMetadata(typeof(OutlookBarControl)));
            }
        }

        public static readonly DependencyProperty OutlookBarMetadataProperty = DependencyProperty.RegisterAttached(
            "OutlookBarMetadata",
            typeof(IOutlookBarMetadata),
            typeof(OutlookBarControl));

        public static void SetOutlookBarMetadata(DependencyObject viewTarget, IOutlookBarMetadata outlookBarMetadata)
        {
            viewTarget.SetValue(OutlookBarMetadataProperty, outlookBarMetadata);
        }

        public static IOutlookBarMetadata GetOutlookBarMetadata(DependencyObject viewTarget)
        {
            return viewTarget.GetValue(OutlookBarMetadataProperty) as IOutlookBarMetadata;
        }
    }
}