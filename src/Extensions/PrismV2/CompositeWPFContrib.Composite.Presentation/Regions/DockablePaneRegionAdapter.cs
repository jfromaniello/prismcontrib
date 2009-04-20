using System.Collections.Specialized;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Regions;

using AvalonDock;

namespace CompositeWPFContrib.Composite.Presentation.Regions
{
    /// <summary>
    /// RegionAdapter that creates a new <see cref="Region"/> and binds all
    /// the views to the adapted <see cref="DockingManager"/> or <see cref="ResizingPanel"/>.
    /// Just insert a XAML-Tag xmlns:DockablePane into your XAML-file (e.g. Shell.xaml).
    /// The xml namespace must be previously defined (e.g. xmlns:AvalonDock=...)
    /// 
    /// 
    /// created 18.02.2009 by Markus Raufer
    /// </summary>
    public class DockablePaneRegionAdapter : RegionAdapterBase<DockablePane>
    {
        public delegate void DockableContentHandler();

        /// <summary>
        /// This is the default constructor.
        /// Let the container do the creation of the RegionBehaviorFactory.
        /// Just get it by injection.
        /// </summary>
        /// <param name="regionBehaviorFactory"></param>
        public DockablePaneRegionAdapter( IRegionBehaviorFactory regionBehaviorFactory ) : base( regionBehaviorFactory )
        {
        }
        /// <summary>
        /// The new region which will be placed at the palceholder in the XAML-file.
        /// </summary>
        /// <returns></returns>
        protected override IRegion CreateRegion()
        {
            return new Region();
        }

        /// <summary>
        /// This method will be called, each time the parser encounters a XAML-Tag xmlns:DockablePane.
        /// </summary>
        /// <param name="region">The region with the encountered DockablePane.</param>
        /// <param name="regionTarget">The encountered DockablePane.</param>
        protected override void Adapt(IRegion region, DockablePane regionTarget)
        {
            region.Views.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
            {
                OnViewsCollectionChanged(sender, e, region, regionTarget);
            };
        }

        /// <summary>
        /// Adds the DockableContent to the DockablePane.
        /// This method will be called on each change of the regions view collection.
        /// </summary>
        /// <param name="sender">The regions view collection.</param>
        /// <param name="e">The arguments of the notification event.</param>
        /// <param name="region">The region with the view collection.</param>
        /// <param name="regionTarget">The DockablePane where the Content should be added.</param>
        private void OnViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e, IRegion region, DockablePane regionTarget)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (object item in e.NewItems)
                    {
                        var newContent = item as DockableContent;

                        if (newContent != null)
                        {
                            regionTarget.Items.Add(newContent);
                            newContent.InvalidateParents();
                        }
                    }
                    break;
            }
        }
    }
}
