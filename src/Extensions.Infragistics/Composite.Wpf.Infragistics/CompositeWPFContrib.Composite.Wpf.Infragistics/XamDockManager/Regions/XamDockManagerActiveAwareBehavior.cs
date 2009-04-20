using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infragistics.Windows.DockManager;
using Microsoft.Practices.Composite.Regions;
using System.Windows;
using System.Collections.Specialized;
using Microsoft.Practices.Composite.Events;
using CompositeWPFContrib.Composite.Wpf.Infragistics.Extensions;
using CompositeWPFContrib.Composite.Wpf.Infragistics.DockManager.Metadata;


namespace CompositeWPFContrib.Composite.Wpf.Infragistics.DockManager.Regions
{
    /// <summary>
    /// Keeps the <see cref="IRegion.ActiveViews"/> and ContentPanes of the  <see cref="XamDockManager"/> in sync.    
    /// </summary>
    public class XamDockManagerActiveAwareBehavior
    {
        //The behavior uses weak references while listening to events to prevent memory leaks
        //when destroying the region but not the control or viceversa.
        private readonly WeakReference _regionWeakReference;
        private readonly WeakReference _xamDockManagerWeakReference;


        public XamDockManagerActiveAwareBehavior(XamDockManager xamDockManager, IRegion region)
            {
                _regionWeakReference = new WeakReference(region);

                _xamDockManagerWeakReference = new WeakReference(xamDockManager);
            }

            internal void Attach()
            {
                IRegion region = GetRegion();
                XamDockManager xamDockManager = GetDockManager();

                if (region != null && xamDockManager != null)
                {
                    region.ActiveViews.CollectionChanged += OnActiveViewsChanged;
                    xamDockManager.ActivePaneChanged += OnActivePaneChanged;
                }
            }

            internal void Detach()
            {
                XamDockManager xamDockManager = GetDockManager();
                if (xamDockManager != null)
                {
                    xamDockManager.ActivePaneChanged -= OnActivePaneChanged;
                }

                IRegion region = GetRegion();
                if (region != null)
                {
                    region.ActiveViews.CollectionChanged -= OnActiveViewsChanged;
                }
            }

            private void OnActivePaneChanged(object sender, RoutedPropertyChangedEventArgs<ContentPane> args)
            {
                IRegion region = GetRegion();

                XamDockManager xamDockManager = GetDockManager();
                
                if (region == null || xamDockManager == null)
                {
                    Detach();
                }
                else
                {
                    if (xamDockManager == args.OriginalSource)
                    {
                        if (args.OldValue != null)
                        {
                            Object view = args.OldValue.Content;
                            if (view != null)
                            {
                                if (region.Views.Contains(view))
                                {
                                    region.Deactivate(view);
                                }
                            }

                        }

                        if (args.NewValue != null)
                        {
                            Object view = args.NewValue.Content;
                            if (view != null)
                            {
                                if (region.Views.Contains(view))
                                {
                                    region.Activate(view);
                                }
                            }
                        }
                    }
                }
            }

            private void OnActiveViewsChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                IRegion region = GetRegion();
                XamDockManager xamDockManager = GetDockManager();
                if (region == null || xamDockManager == null)
                {
                    Detach();
                }
                else
                {
                    if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        IEnumerable<ContentPane> contentPanes = xamDockManager.GetPanes(PaneNavigationOrder.VisibleOrder);

                        foreach (ContentPane contentPane in contentPanes)
                        {
                            if (xamDockManager.ActivePane != contentPane)
                            {
                                if (contentPane.Content == e.NewItems[0])
                                {
                                    contentPane.Activate();
                                    //If content pane is part of TabGroupPane then by activating it, makes it automatically selected.
                                }
                            }
                        }
                    }
                    else if (e.Action == NotifyCollectionChangedAction.Remove)
                    {
                        //TODO see if is necessary to Deactivate the ActivePane
                        //One way is by focusing other framework element. 
                        //Window win = xamDockManager.FindWindow();
                        //if (win!=null)
                        //    win.Focus();
                    }
                }
            }

            private XamDockManager GetDockManager()
            {
                return _xamDockManagerWeakReference.Target as XamDockManager;
            }
          

            private IRegion GetRegion()
            {
                return _regionWeakReference.Target as IRegion;
            }
    }
}
