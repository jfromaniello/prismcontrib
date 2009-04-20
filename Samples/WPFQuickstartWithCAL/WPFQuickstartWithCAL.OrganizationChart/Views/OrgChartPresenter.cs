using System;
using System.Xml;
using System.Globalization;
using Microsoft.Practices.Composite.Events;
using WPFQuickstartWithCAL.Infrastructure.Events;

namespace WPFQuickstartWithCAL.OrganizationChart.Views
{
    public class OrgChartPresenter
    {
        private readonly IEventAggregator _eventAggregator;

        public OrgChartPresenter(IOrgChartView view, IEventAggregator eventAggregator)
        {
            this.View = view;
            this._eventAggregator = eventAggregator;

            this.View.PositionSelected += delegate(object sender, DataEventArgs<XmlElement> args)
              {
                  if (_eventAggregator != null)
                  {
                      int selectedEmployeeId = Int32.Parse(args.Value.Attributes["ID"].Value, CultureInfo.InvariantCulture);

                      _eventAggregator.GetEvent<ShowEmployeeEvent>().Publish(selectedEmployeeId);
                  }
              };

            XmlDocument chart = new XmlDocument();
            chart.Load(@"OrganizationChart.xml");
            View.Model = chart;
        }

        public IOrgChartView View { get; set; }
    }
}
