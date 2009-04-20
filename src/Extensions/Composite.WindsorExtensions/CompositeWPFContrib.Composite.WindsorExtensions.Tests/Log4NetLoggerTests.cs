using CompositeWPFContrib.Composite.WindsorExtensions.Tests.Mocks;
using log4net.Config;
using log4net.Layout;
using Microsoft.Practices.Composite.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompositeWPFContrib.Composite.WindsorExtensions.Tests
{
    /// <summary>
    /// Summary description for Log4NetLoggerTests
    /// </summary>
    [TestClass]
    public class Log4NetLoggerTests
    {
        private readonly StringAppenderStub appender;

        public Log4NetLoggerTests()
        {
            appender = new StringAppenderStub { Layout = new SimpleLayout() };
            BasicConfigurator.Configure(appender);
        }

        [TestMethod]
        public void CanWriteToLoggerInDebugCategory()
        {
            ILoggerFacade logger = new Log4NetLogger();

            logger.Log("Test", Category.Debug, Priority.Low);

            string output = appender.GetString();
            StringAssert.Contains(output, "Test");
            StringAssert.Contains(output, "DEBUG");
        }

        [TestMethod]
        public void CanWriteToLoggerInErrorCategory()
        {
            ILoggerFacade logger = new Log4NetLogger();

            logger.Log("Test", Category.Exception, Priority.Low);

            string output = appender.GetString();
            StringAssert.Contains(output, "Test");
            StringAssert.Contains(output, "ERROR");
        }

        [TestMethod]
        public void CanWriteToLoggerInInfoCategory()
        {
            ILoggerFacade logger = new Log4NetLogger();

            logger.Log("Test", Category.Info, Priority.Low);

            string output = appender.GetString();
            StringAssert.Contains(output, "Test");
            StringAssert.Contains(output, "INFO");
        }

        [TestMethod]
        public void CanWriteToLoggerInWarnCategory()
        {
            ILoggerFacade logger = new Log4NetLogger();

            logger.Log("Test", Category.Warn, Priority.Low);

            string output = appender.GetString();
            StringAssert.Contains(output, "Test");
            StringAssert.Contains(output, "WARN");
        }
    }
}
