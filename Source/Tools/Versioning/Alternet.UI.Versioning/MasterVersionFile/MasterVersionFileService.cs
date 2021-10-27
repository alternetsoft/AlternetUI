using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Alternet.UI.Versioning
{
    static class MasterVersionFileService
    {
        private static Regex informationalVersionRegex = new Regex(@"^(?<major>\d+)\.(?<minor>\d+)\.(?<build>\d+) \(\d+\.\d+\ (?<type>\w+) build \d+\)$");

        public static ProductVersion GetVersion(string versionFilePath)
        {
            var doc = XDocument.Load(versionFilePath);
            var informationalVersion = GetInformationalVersionElement(doc);
            return GetVersion(informationalVersion);
        }

        private static XElement GetInformationalVersionElement(XDocument doc)
        {
            XNamespace ns = XmlNamespaces.MSBuild;
            var informationalVersion = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "InformationalVersion").Single();
            return informationalVersion;
        }

        public static int GetBuildNumber(string versionFilePath)
        {
            var doc = XDocument.Load(versionFilePath);

            var informationalVersion = GetInformationalVersionElement(doc);
            return GetBuildNumber(informationalVersion);
        }

        private static ProductVersion GetVersion(XElement informationalVersion)
        {
            var match = informationalVersionRegex.Match(informationalVersion.Value);
            if (!match.Success)
                throw new FormatException();

            return new ProductVersion(
                int.Parse(match.Groups["major"].Value),
                int.Parse(match.Groups["minor"].Value),
                (VersionType)Enum.Parse(typeof(VersionType), match.Groups["type"].Value));
        }

        private static int GetBuildNumber(XElement informationalVersion)
        {
            var match = informationalVersionRegex.Match(informationalVersion.Value);
            if (!match.Success)
                throw new FormatException();

            return int.Parse(match.Groups["build"].Value);
        }

        public static void SetVersion(string versionFilePath, ProductVersion productVersion)
        {
            var doc = XDocument.Load(versionFilePath);

            XNamespace ns = XmlNamespaces.MSBuild;
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