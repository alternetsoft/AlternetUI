using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Alternet.UI.Versioning
{
    public static class VersionReader
    {
        private static Regex informationalVersionRegex = new Regex(@"^(?<major>\d+)\.(?<minor>\d+)\.\d+ \(\d+\.\d+\ (?<type>\w+) build \d+\)$");

        public static ProductVersion GetVersion(string versionFilePath)
        {
            var doc = XDocument.Load(versionFilePath);

            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            var informationalVersion = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "InformationalVersion").Single();
            return GetVersion(informationalVersion);
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
    }
}