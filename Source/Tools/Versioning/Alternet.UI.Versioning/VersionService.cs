using System;
using System.Linq;
using System.Xml.Linq;

namespace Alternet.UI.Versioning
{
    public static class VersionService
    {
        public static void SetVersion(Repository repository, ProductVersion productVersion)
        {
            MasterVersionFileService.SetVersion(GetVersionFilePath(repository), productVersion);
        }

        private static string GetVersionFilePath(Repository repository) => VersionFileLocator.LocateVersionFile(repository);

        public static ProductVersion GetVersion(Repository repository) => MasterVersionFileService.GetVersion(GetVersionFilePath(repository));
    }
}