using System;
using System.IO;
using System.IO.Pipes;
using System.Reflection;

namespace Alternet.UI.Build.Tasks
{
    internal static class WellKnownApiInfo
    {
        private static ApiInfoProvider? provider;

        private static Stream GetProviderStreamOld()
        {
            return typeof(WellKnownApiInfo).Assembly.GetManifestResourceStream("WellKnownApiInfo.xml");
        }

        public static string GetXmlPath()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string s = Path.GetDirectoryName(location)!;
            const string XmlFileName = "Alternet.UI.AllClasses.xml";

            s = Path.Combine(s, "..", "..", "..", "..", "Alternet.UI", "bin", XmlFileName);

            s = Path.GetFullPath(s);

            return s;
        }

        private static Stream GetProviderStreamNew()
        {
            using var fileStream = new FileStream(GetXmlPath(), FileMode.Open, FileAccess.Read);
            var memStream = new MemoryStream();
            fileStream.CopyTo(memStream);
            fileStream.Close();

            return memStream;
        }

        private static Stream GetProviderStream()
        {
            return GetProviderStreamOld();
        }

        public static ApiInfoProvider Provider =>
            provider ??= new ApiInfoProvider(
                GetProviderStream() ?? throw new Exception());
    }
}