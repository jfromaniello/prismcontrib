using System.Text;
using log4net.Appender;
using log4net.Core;

namespace CompositeWPFContrib.Composite.WindsorExtensions.Tests.Mocks
{
    public class StringAppenderStub : AppenderSkeleton
    {
        private readonly StringBuilder output = new StringBuilder();

        public string GetString()
        {
            return output.ToString();
        }

        public void Reset()
        {
            output.Length = 0;
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            string append = RenderLoggingEvent(loggingEvent);
            output.Append(append);
        }

        protected override bool RequiresLayout
        {
            get { return false; }
        }
    }
}
