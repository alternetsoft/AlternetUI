using Serilog;
using System.IO;

namespace Alternet.UI.Integration.UIXmlHostApp.Remote
{
    internal static class ResourceLocator
    {
        public static string TempWorkingDirectory
        {
            get
            {
                var path = Path.Combine(Path.GetTempPath(), "AlterNET.UIXMLPreviewer");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path;
            }
        }

        public static string ScreenshotsDirectory
        {
            get
            {
                var path = Path.Combine(TempWorkingDirectory, "Screenshots");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path;
            }
        }

        public static string LogsDirectory
        {
            get
            {
                var path = Path.Combine(TempWorkingDirectory, "Logs");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path;
            }
        }
    }
}