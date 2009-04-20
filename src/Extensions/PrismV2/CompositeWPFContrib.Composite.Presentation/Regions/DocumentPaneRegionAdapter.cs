using System.Collections.Specialized;
using AvalonDock;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Regions;

namespace CompositeWPFContrib.Composite.Presentation.Regions
{
    /// <summary>
    /// created 18.02.2009 by Markus Raufer
    /// </summary>
    public class DocumentPaneRegionAdapter : RegionAdapterBase<DocumentPane>
    {
        public delegate void DockableContentHandler();

        public DocumentPaneRegionAdapter( IRegionBehaviorFactory regionBehaviorFactory ) : base( regionBehaviorFactory )
        {
        }

        protected override IRegion CreateRegion()
        {
            return new Region();
        }

        protected override void Adapt(IRegion region, DocumentPane regionTarget)
        {
            region.Views.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
            {
                OnViewsCollectionChanged(sender, e, region, regionTarget);
            };
        }

        private void OnViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e, IRegion region, DocumentPane regionTarget)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (object item in e.NewItems)
                    {
                        var newContent = item as DocumentContent;

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
