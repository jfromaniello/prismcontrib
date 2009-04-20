using System.Collections.Generic;
using Microsoft.Practices.Composite.Logging;

namespace CompositeWPFContrib.Composite.WindsorExtensions.Tests.Mocks
{
    public class MockLoggerAdapter : ILoggerFacade
    {
        public IList<string> Messages = new List<string>();

        public void Log(string message, Category category, Priority priority)
        {
            Messages.Add(message);
        }
    }
}
