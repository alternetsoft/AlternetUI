using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Alternet.UI.Versioning
{
    public static class VersionService
    {
        public static void SetVersion(
            Repository repository,
            ProductVersion productVersion,
            int buildNumber)
        {
            var locator = new FileLocator(repository);

            MasterVersionFileService.SetVersion(locator.GetMasterVersionFile(), productVersion);

            PatchProjectFiles(productVersion, buildNumber, locator);
            PatchVSPackageManifests(productVersion, locator);
            PatchDocumentationExamplesProjectFiles(productVersion, locator);
        }

        internal static void PatchPalProjectFile(
            ProductVersion productVersion,
            int buildNumber,
            FileLocator locator)
        {
            PatchVersionInPalFiles(
                locator.GetPalProjectFiles(),
                productVersion,
                buildNumber);
        }

        static void PatchVersionInPalFiles(IEnumerable<string> files, ProductVersion productVersion, int buildNumber)
        {
            var patcher = new XmlPatcher(omitXmlDeclaration: false);
            foreach (var file in files)
                patcher.PatchValue(
                    file,
                    "/msb:Project/msb:PropertyGroup/msb:MyFileVersion",
                    productVersion.GetMajorMinorAndBuild(buildNumber));
        }

        internal static void PatchProjectFiles(
            ProductVersion productVersion,
            int buildNumber,
            FileLocator locator)
        {
            PatchVersionInProjectFiles(
                locator.GetProjectFiles(),
                productVersion.GetPackageVersion(buildNumber));
        }

        internal static void PatchDocumentationExamplesProjectFiles(
            ProductVersion productVersion,
            FileLocator locator)
        {
            PatchVersionInProjectFiles(locator.GetDocumentationExamplesProjectFiles(), productVersion.GetPackageFloatingReferenceVersion());
        }

        static void PatchVersionInProjectFiles(IEnumerable<string> files, string version)
        {
            var patcher = new XmlPatcher(omitXmlDeclaration: true);
            foreach (var file in files)
                patcher.PatchAttribute(
                    file,
                    "/Project/ItemGroup/PackageReference[@Include='Alternet.UI']",
                    "Version",
                    version);
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

        public static ProductVersion GetVersion(Repository repository)
            => MasterVersionFileService.GetVersion(new FileLocator(repository).GetMasterVersionFile());
        public static int GetBuildNumber(Repository repository)
            => MasterVersionFileService.GetBuildNumber(new FileLocator(repository).GetMasterVersionFile());
    }
}