using System;
using System.Xml;
using Microsoft.Practices.Composite.Events;

namespace WPFQuickstartWithCAL.OrganizationChart.Views
{
    public interface IOrgChartView
    {
        event EventHandler<DataEventArgs<XmlElement>> PositionSelected;
        XmlDocument Model { get; set; }
    }
}
