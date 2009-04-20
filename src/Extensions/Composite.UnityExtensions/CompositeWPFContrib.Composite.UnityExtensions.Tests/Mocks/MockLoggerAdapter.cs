using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Logging;

namespace CompositeWPFContrib.Composite.UnityExtensions.Tests.Mocks
{
    internal class MockLoggerAdapter : ILoggerFacade
    {
        public IList<string> Messages = new List<string>();

        public void Log(string message, Category category, Priority priority)
        {
            Messages.Add(message);
        }
    }
}
