using System.IO;

namespace Alternet.UI.Integration.UIXmlHostApp.Remote
{
    internal static class Logger
    {
        public static void Log(string message)
        {
#if DEBUG
            if (Directory.Exists(@"c:\temp"))
                File.AppendAllText(@"c:\temp\UIXMLPreviewerProcess.log", message + "\n");
#endif
        }
    }
}