using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TmaxDependencyDownloader
{
    public static class LoggerManager
    {
        #region Private Members
        private static readonly ILog log;
        #endregion

        #region Static Constructor
        static LoggerManager()
        {
            log = LogManager.GetLogger(typeof(LoggerManager));
            XmlConfigurator.Configure();
        }
        #endregion

        #region Public Methods
        public static void LogFatal(Exception ex)
        {
            log.Fatal(ex.ToString());
        }

        public static void LogError(Exception ex)
        {
            log.Error(ex.ToString());
        }
        
        public static void LogError(string message)
        {
            log.Error(message);
        }
        public static void LogInfo(string info)
        {
            log.Info(info);
        }
        #endregion
    }
}
