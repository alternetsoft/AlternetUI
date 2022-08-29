using Serilog;
using System.IO;

namespace Alternet.UI.Integration.UIXmlHostApp.Remote
{
    internal static class Logger
    {
        private static Serilog.Core.Logger logger;

        public static Serilog.Core.Logger Instance
        {
            get
            {
                if (logger == null)
                {
                    logger = new LoggerConfiguration()
                      .MinimumLevel.Debug()
                      .WriteTo.File(Path.Combine(ResourceLocator.LogsDirectory, @"UIXmlHostApp.log"), rollingInterval: RollingInterval.Day)
                      .CreateLogger();
                }

                return logger;
            }
        }
    }
}