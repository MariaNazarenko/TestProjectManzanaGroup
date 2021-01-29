using log4net;
using log4net.Config;

namespace WindowsService
{
    /// <summary>
    /// Класс для рыботы с log4net
    /// </summary>
    public sealed class Logger
    {
        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        public static ILog Log
        {
            get { return log; }
        }

        public static void InitLogger()
        {
            XmlConfigurator.Configure();
        }
    }
}
