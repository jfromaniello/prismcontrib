using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using CompositeWPFContrib.Composite.Wpf.Controls;
using CompositeWPFContrib.Composite.Wpf.Events;
using CompositeWPFContrib.Composite.Wpf.Properties;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Regions;

namespace CompositeWPFContrib.Composite.Wpf.Regions
{
    public class OutlookBarControlRegionAdapter : SelectorRegionAdapter
    {
        private readonly IEventAggregator eventAggregator;

        public OutlookBarControlRegionAdapter(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        protected override void AttachBehaviors(IRegion region, Selector regionTarget)
        {
            base.AttachBehaviors(region, regionTarget);

            OutlookBarPublishBehavior behavior = new OutlookBarPublishBehavior(regionTarget, eventAggregator);
            behavior.Attach();
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }

        protected override void Adapt(IRegion region, Selector regionTarget)
        {
            if (!(regionTarget is OutlookBarControl))
                throw new ArgumentException(Resources.RegionTargetNotValidException);

            base.Adapt(region, regionTarget);
        }
    }

    internal class OutlookBarPublishBehavior
    {
        private readonly WeakReference _selectorWeakReference;
        private readonly IEventAggregator eventAggregator;

        public OutlookBarPublishBehavior(Selector selector, IEventAggregator eventAggregator)
        {
            this._selectorWeakReference = new WeakReference(selector);
            this.eventAggregator = eventAggregator;
        }

        public void Attach()
        {
            Selector selector = GetSelector();
            if (selector != null)
            {
                selector.SelectionChanged += OnSelectionChanged;
            }
        }

        void OnSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Selector selector = GetSelector();
            if (selector == null)
            {
                Detach();
            }
            else
            {
                foreach (object item in e.AddedItems)
                {
                    DependencyObject target = item as DependencyObject;
                    if (target != null)
                    {
                        IOutlookBarMetadata metadata = OutlookBarControl.GetOutlookBarMetadata(target);
                        string payload = metadata != null ? metadata.Payload : null;

                        eventAggregator.GetEvent<OutlookBarEvent>().Publish(payload);
                    }
                }
            }
        }

        private void Detach()
        {
            Selector selector = GetSelector();
            if (selector != null)
            {
                selector.SelectionChanged -= OnSelectionChanged;
            }
        }

        private Selector GetSelector()
        {
            return _selectorWeakReference.Target as Selector;
        }
    }
}