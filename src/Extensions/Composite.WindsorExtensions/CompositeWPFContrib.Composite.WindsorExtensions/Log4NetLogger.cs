using System.Reflection;
using log4net;
using Microsoft.Practices.Composite.Logging;

namespace CompositeWPFContrib.Composite.WindsorExtensions
{
    public class Log4NetLogger : ILoggerFacade
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Debug:
                    logger.Debug(message);
                    break;

                case Category.Exception:
                    logger.Error(message);
                    break;

                case Category.Warn:
                    logger.Warn(message);
                    break;

                default:
                    logger.Info(message);
                    break;
            }
        }
    }
}
