using System;
using System.Linq;
using System.Xml.Linq;

namespace Alternet.UI.Versioning
{
    public static class VersionService
    {
        public static void SetVersion(Repository repository, ProductVersion productVersion)
        {
            var locator = new FileLocator(repository);

            MasterVersionFileService.SetVersion(locator.GetMasterVersionFile(), productVersion);

            foreach (var file in locator.GetTemplateProjectFiles())
                XmlPatcher.PatchAttribute(
                    file,
                    "/Project/ItemGroup/PackageReference[@Include='Alternet.UI']",
                    "Version",
                    productVersion.GetPackageReferenceVersion());
        }

        public static ProductVersion GetVersion(Repository repository) => MasterVersionFileService.GetVersion(new FileLocator(repository).GetMasterVersionFile());
    }
}