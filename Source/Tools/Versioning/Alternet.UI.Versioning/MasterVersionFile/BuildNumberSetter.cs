using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Alternet.UI.Versioning
{
    public static class BuildNumberSetter
    {
        private static Regex informationalVersionRegex = new Regex(@"^\d+\.\d+\.(?<build1>\d+) \(\d+\.\d+\ \w+ build (?<build2>\d+)\)$");

        public static void SetBuildNumber(Repository repository, int buildNumber)
        {
            Console.WriteLine($"Increasing build number...");

            var versionFilePath = new FileLocator(repository).GetMasterVersionFile();

            var doc = XDocument.Load(versionFilePath);

            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            var informationalVersion = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "InformationalVersion").Single();
            TrySetBuildNumber(informationalVersion, x => TrySetBuildNumberInInformationalVersion(x, buildNumber));

            var version = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "Version").Single();
            TrySetBuildNumber(version, x => TrySetBuildNumberInPackageVersion(x, buildNumber));

            var packageVersion = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "PackageVersion").Single();
            TrySetBuildNumber(packageVersion, x => TrySetBuildNumberInPackageVersion(x, buildNumber));

            var assemblyVersion = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "AssemblyVersion").Single();
            TrySetBuildNumber(assemblyVersion, x => TrySetBuildNumberInAssemblyVersion(x, buildNumber));

            var fileVersion = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "FileVersion").Single();
            TrySetBuildNumber(fileVersion, x => TrySetBuildNumberInAssemblyVersion(x, buildNumber));

            doc.Save(versionFilePath);
        }

        private static string TrySetBuildNumber(Regex regex, string value, int buildNumber, string groupName)
        {
            var match = regex.Match(value);
            if (!match.Success)
                return value;

            if (!match.Groups.TryGetValue(groupName, out var group))
                return value;
            if (!int.TryParse(group.Value, out var _))
                return value;

            return ReplaceNamedGroup(groupName, buildNumber.ToString(), match);
        }

        private static string TrySetBuildNumberInInformationalVersion(string value, int buildNumber)
        {
            var v = TrySetBuildNumber(informationalVersionRegex, value, buildNumber, "build1");
            return TrySetBuildNumber(informationalVersionRegex, v, buildNumber, "build2");
        }

        private static string TrySetBuildNumberInPackageVersion(string value, int buildNumber) =>
            TrySetBuildNumber(new Regex(@"^\d+\.\d+\.(?<build>\d+)(-\w+)*$"), value, buildNumber, "build");

        private static string TrySetBuildNumberInAssemblyVersion(string value, int buildNumber) =>
            TrySetBuildNumber(new Regex(@"^\d+\.\d+\.(?<build>\d+).\d+$"), value, buildNumber, "build");

        private static string ReplaceNamedGroup(string groupName, string replacement, Match m)
        {
            string capture = m.Value;
            capture = capture.Remove(m.Groups[groupName].Index - m.Index, m.Groups[groupName].Length);
            capture = capture.Insert(m.Groups[groupName].Index - m.Index, replacement);
            return capture;
        }

        private static void TrySetBuildNumber(XElement informationalVersion, Func<string, string> increaser)
        {
            var oldInformationalVersionValue = informationalVersion.Value;
            informationalVersion.Value = increaser(informationalVersion.Value);
            if (oldInformationalVersionValue != informationalVersion.Value)
                Console.WriteLine($"Changing from {oldInformationalVersionValue} to {informationalVersion.Value}");
        }
    }
}