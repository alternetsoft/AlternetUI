using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Alternet.UI.Versioning
{
    public static class VersionService
    {
        public static void SetVersion(Repository repository, ProductVersion productVersion, int buildNumber)
        {
            var locator = new FileLocator(repository);

            MasterVersionFileService.SetVersion(locator.GetMasterVersionFile(), productVersion);

            PatchTemplateProjectFiles(productVersion, buildNumber, locator);
            PatchVSPackageManifests(productVersion, locator);
        }

        internal static void PatchTemplateProjectFiles(ProductVersion productVersion, int buildNumber, FileLocator locator)
        {
            var patcher = new XmlPatcher(omitXmlDeclaration: true);
            foreach (var file in locator.GetTemplateProjectFiles())
                patcher.PatchAttribute(
                    file,
                    "/Project/ItemGroup/PackageReference[@Include='Alternet.UI']",
                    "Version",
                    productVersion.GetPackageVersion(buildNumber));
        }

        static void PatchVSPackageManifests(ProductVersion productVersion, FileLocator locator)
        {
            var patcher = new XmlPatcher(omitXmlDeclaration: false);
            foreach (var file in locator.GetVSPackageManifestFiles())
            {
                patcher.PatchAttribute(
                    file,
                    $"/x:PackageManifest/x:Metadata/x:Identity",
                    "Version",
                    productVersion.GetAssemblyVersion(),
                    new Dictionary<string, string> { { "x", XmlNamespaces.VSManifest.NamespaceName } });
            }
        }

        public static ProductVersion GetVersion(Repository repository) => MasterVersionFileService.GetVersion(new FileLocator(repository).GetMasterVersionFile());
        public static int GetBuildNumber(Repository repository) => MasterVersionFileService.GetBuildNumber(new FileLocator(repository).GetMasterVersionFile());
    }
}