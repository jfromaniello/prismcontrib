using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls.Primitives;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Regions;

namespace CompositeWPFContrib.Composite.Wpf.Regions
{
    /// <summary>
    /// Adapter that creates a new <see cref="AllActiveRegion"/> and binds all
    /// the views to the adapted <see cref="ToolBarPanel"/>. 
    /// </summary>
    public class ToolBarPanelRegionAdapter : RegionAdapterBase<ToolBarPanel>
    {
        /// <summary>
        /// Adapts a <see cref="ToolBarPanel"/> and binds all active views to the <see cref="ToolBarPanel"/>
        /// </summary>
        /// <param name="region">The region to adapt.</param>
        /// <param name="regionTarget">The <see cref="ToolBarPanel"/> to bind the active views to</param>
        protected override void Adapt(IRegion region, ToolBarPanel regionTarget)
        {
            if (region == null)
                throw new ArgumentNullException("region");

            if (regionTarget == null)
                throw new ArgumentNullException("regionTarget");

            foreach (UIElement element in regionTarget.Children)
            {
                region.Add(element);
            }

            region.ActiveViews.CollectionChanged +=
                new NotifyCollectionChangedEventHandler((sender, e) =>
                {
                    if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        foreach (object item in e.NewItems)
                        {
                            UIElement element = item as UIElement;
                            if (element != null)
                            {
                                regionTarget.Children.Add(element);
                            }
                        }
                    }
                    else
                    {
                        if (e.Action == NotifyCollectionChangedAction.Remove)
                        {
                            foreach (object item in e.NewItems)
                            {
                                UIElement element = item as UIElement;

                                if (element != null)
                                {
                                    regionTarget.Children.Remove(element);
                                }
                            }
                        }
                    }
                });
        }

        /// <summary>
        /// Creates a new instance of <see cref="AllActiveRegion"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="AllActiveRegion"/>.</returns>
        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }
    }
}