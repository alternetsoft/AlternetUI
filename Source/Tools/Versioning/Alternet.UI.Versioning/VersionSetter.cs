using System;
using System.Linq;
using System.Xml.Linq;

namespace Alternet.UI.Versioning
{
    public static class VersionSetter
    {
        public static void SetVersion(string versionFilePath, ProductVersion productVersion)
        {
            var doc = XDocument.Load(versionFilePath);

            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            var informationalVersion = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "InformationalVersion").Single();
            informationalVersion.Value = productVersion.GetInformationalVersion();

            var packageVersion = productVersion.GetPackageVersion();
            var version = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "Version").Single();
            version.Value = packageVersion;

            var packageVersionElement = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "PackageVersion").Single();
            packageVersionElement.Value = packageVersion;

            var assemblyVersion = productVersion.GetAssemblyVersion();

            var assemblyVersionElement = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "AssemblyVersion").Single();
            assemblyVersionElement.Value = assemblyVersion;

            var fileVersionElement = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "FileVersion").Single();
            fileVersionElement.Value = assemblyVersion;

            doc.Save(versionFilePath);
        }
    }
}