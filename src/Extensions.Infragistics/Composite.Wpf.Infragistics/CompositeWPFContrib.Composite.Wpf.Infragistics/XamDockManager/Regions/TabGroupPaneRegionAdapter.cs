using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using CompositeWPFContrib.Composite.Wpf.Infragistics.Extensions;
using Infragistics.Windows.DockManager;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Regions;
using Infragistics.Windows.DockManager.Events;

namespace CompositeWPFContrib.Composite.Wpf.Infragistics.DockManager.Regions
{
    /// <summary>
    /// Adapter that creates a new <see cref="Region"/> and binds all
    /// the views to the adapted <see cref="TabGroupPane"/>.
    /// It also keeps the <see cref="IRegion.ActiveViews"/> and the content panes
    /// of the <see cref="XamDockManager"/> in sync.
    /// </summary>
    public class TabGroupPaneRegionAdapter : RegionAdapterBase<TabGroupPane>
    {
        /// <summary>
        /// Adapts an <see cref="TabGroupPane"/> to an <see cref="IRegion"/>.
        /// </summary>
        /// <param name="region">The new region being used.</param>
        /// <param name="regionTarget">The object to adapt.</param>
        protected override void Adapt(IRegion region, TabGroupPane regionTarget)
        {
            if (region == null)
                throw new ArgumentNullException("region");

            if (regionTarget == null)
                throw new ArgumentNullException("regionTarget");

            if (regionTarget.FindDockManager() == null)
                throw new ArgumentException("TabGroupPane must be nested in a XamDockManager control");

            region.Views.CollectionChanged += delegate(Object sender, NotifyCollectionChangedEventArgs e)
            {
                OnViewsCollectionChanged(sender, e, region,regionTarget);
            };
        }

        private void OnViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e, IRegion region, TabGroupPane regionTarget)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                //Add content panes for each associated view. 
                foreach (object item in e.NewItems)
                {
                    UIElement view = item as UIElement;

                    if (view != null)
                    {
                        ContentPane newContentPane = new ContentPane();
                        newContentPane.Content = item;
                        //if associated view has metadata then apply it.
                        if (view.GetTabGroupPaneMetadata() != null)
                        {
                            newContentPane.Header = (view.GetTabGroupPaneMetadata()).Header;
                        }
                        //When contentPane is closed remove the associated region 
                        newContentPane.Closed += delegate(object contentPaneSender, PaneClosedEventArgs args)
                        {
                            OnContentPaneClosed((ContentPane)contentPaneSender, args, region);
                        };


                        regionTarget.Items.Add(newContentPane);
                    }
                }
            }
            else
            {
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    //Associated View has been removed => remove the associated ContentPane from XamDockManager
                    XamDockManager xamDockManager = regionTarget.FindDockManager();
                    IEnumerable<ContentPane> contentPanes = xamDockManager.GetPanes(PaneNavigationOrder.VisibleOrder);

                    foreach (ContentPane contentPane in contentPanes)
                    {
                        if (e.OldItems.Contains(contentPane.Content))
                        {
                            contentPane.Content = null;
                            contentPane.CloseAction = PaneCloseAction.RemovePane;
                            contentPane.ExecuteCommand(ContentPaneCommands.Close);
                        }
                    }


                }
            }
        }

        private void OnContentPaneClosed(ContentPane contentPane, PaneClosedEventArgs args, IRegion region)
        {
            object view = contentPane.Content;
            if (region.Views.Contains(view))
            {
                region.Remove(view);
            }
        }

        /// <summary>
        /// Attach new behaviors.
        /// </summary>
        /// <param name="region">The region being used.</param>
        /// <param name="regionTarget">The object to adapt.</param>
        /// <remarks>
        /// <see cref="XamDockManagerActiveAwareBehavior"/> 
        /// </remarks>
        protected override void AttachBehaviors(IRegion region, TabGroupPane regionTarget)
        {
            base.AttachBehaviors(region, regionTarget);

            XamDockManagerActiveAwareBehavior syncBehavior = new XamDockManagerActiveAwareBehavior(regionTarget.FindDockManager(), region);
            syncBehavior.Attach();
        }

        /// <summary>
        /// Creates a new instance of <see cref="Region"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="Region"/>.</returns>
        protected override IRegion CreateRegion()
        {
            return new Region();
        }

    }
}
