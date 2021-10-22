using System;
using System.Linq;
using System.Xml.Linq;

namespace Alternet.UI.Versioning
{
    public class VersionSetter
    {
        public static void SetVersion(string versionFilePath, ProductVersion productVersion)
        {
            Console.WriteLine($"Increasing build number...");

            var doc = XDocument.Load(versionFilePath);

            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            var informationalVersion = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "InformationalVersion").Single();
            informationalVersion.Value = GetInformationalVersion(productVersion);

            var packageVersion = GetPackageVersion(productVersion);
            var version = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "Version").Single();
            version.Value = packageVersion;

            var packageVersionElement = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "PackageVersion").Single();
            packageVersionElement.Value = packageVersion;

            var assemblyVersion = GetAssemblyVersion(productVersion);

            var assemblyVersionElement = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "AssemblyVersion").Single();
            assemblyVersionElement.Value = assemblyVersion;

            var fileVersionElement = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "FileVersion").Single();
            fileVersionElement.Value = assemblyVersion;

            doc.Save(versionFilePath);
        }

        private static string GetInformationalVersion(ProductVersion version) =>
            $"{version.Major}.{version.Minor}.0 ({version.Major}.{version.Minor} {version.Type} build 0)";

        private static string GetPackageVersion(ProductVersion version)
        {
            var v = $"{version.Major}.{version.Minor}.0";
            if (version.Type == VersionType.Release)
                return v;
            return $"{v}-{version.Type.ToString().ToLower()}";
        }

        private static string GetAssemblyVersion(ProductVersion version) => $"{version.Major}.{version.Minor}.0.0";
    }
}