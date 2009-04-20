using System.Windows.Media;

namespace CompositeWPFContrib.Composite.Wpf
{
    public interface IOutlookBarMetadata
    {
        string Title { get; set; }
        ImageSource Icon { get; set; }
        string Payload { get; set; }
    }
}