using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace VersionTool
{
    internal class BuildNumberSetter
    {
        public static void SetBuildNumber(string versionFilePath, int buildNumber)
        {
            Console.WriteLine($"Increasing build number...");

            var doc = XDocument.Load(versionFilePath);

            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            var informationalVersion = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "InformationalVersion").Single();
            TrySetBuildNumber(informationalVersion, x => TrySetBuildNumberInInformationalVersion(x, buildNumber));

            var packageVersion = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "Version").Single();
            TrySetBuildNumber(packageVersion, x => TrySetBuildNumberInPackageVersion(x, buildNumber));

            doc.Save(versionFilePath);
        }

        private static string TrySetBuildNumber(Regex regex, string value, int buildNumber)
        {
            var match = regex.Match(value);
            if (!match.Success)
                return value;

            if (!match.Groups.TryGetValue("build", out var group))
                return value;
            if (!int.TryParse(group.Value, out var _))
                return value;

            return ReplaceNamedGroup("build", buildNumber.ToString(), match);
        }

        private static string TrySetBuildNumberInInformationalVersion(string value, int buildNumber) =>
            TrySetBuildNumber(new Regex(@"^\d+\.\d+\.\d+ \(\d+\.\d+\.\d+ \w+ build (?<build>\d+)\)$"), value, buildNumber);

        private static string TrySetBuildNumberInPackageVersion(string value, int buildNumber) =>
            TrySetBuildNumber(new Regex(@"^\d+\.\d+\.\d+-\w+\.(?<build>\d+)$"), value, buildNumber);

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