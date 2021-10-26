using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Alternet.UI.Versioning
{
    internal static class VersionFileLocator
    {
        public static string LocateVersionFile(Repository repository)
        {
            var versionFilePath = Path.GetFullPath(Path.Combine(repository.RootPath, "Source/Mastering/Version/Version.props"));
            if (!File.Exists(versionFilePath))
                throw new InvalidOperationException("Cannot locate Version.props file at" + versionFilePath);
            return versionFilePath;
        }
    }
}