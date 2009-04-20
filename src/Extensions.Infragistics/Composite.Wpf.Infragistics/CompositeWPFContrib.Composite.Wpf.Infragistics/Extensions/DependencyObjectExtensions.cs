using System.Windows;
using CompositeWPFContrib.Composite.Wpf.Infragistics.DockManager.Metadata;
using Infragistics.Windows.DockManager;

namespace CompositeWPFContrib.Composite.Wpf.Infragistics.Extensions
{
    /// <summary>
    /// Provides Method Extensions to <see cref="DependencyObject"/> 
    /// </summary>
    public static class DependencyObjectExtensions
    {
        public static readonly DependencyProperty ContentPaneMetadataProperty = DependencyProperty.RegisterAttached(
           "ContentPaneMetadata",
           typeof(IContentPaneMetadata),
           typeof(ContentPane));

        public static readonly DependencyProperty TabGroupPaneMetadataProperty = DependencyProperty.RegisterAttached(
          "TabGroupPaneMetadata",
          typeof(ITabGroupPaneMetadata),
          typeof(TabGroupPane));

        public static void SetContentPaneMetadata(this DependencyObject viewTarget, IContentPaneMetadata contentPaneMetadata)
        {
            viewTarget.SetValue(ContentPaneMetadataProperty, contentPaneMetadata);
        }

        public static IContentPaneMetadata GetContentPaneMetadata(this DependencyObject viewTarget)
        {
            return viewTarget.GetValue(ContentPaneMetadataProperty) as IContentPaneMetadata;
        }

        public static void SetTabGroupPaneMetadata(this DependencyObject viewTarget, ITabGroupPaneMetadata TabGroupPaneMetadata)
        {
            viewTarget.SetValue(TabGroupPaneMetadataProperty, TabGroupPaneMetadata);
        }

        public static ITabGroupPaneMetadata GetTabGroupPaneMetadata(this DependencyObject viewTarget)
        {
            return viewTarget.GetValue(TabGroupPaneMetadataProperty) as ITabGroupPaneMetadata;
        }
    }
}
