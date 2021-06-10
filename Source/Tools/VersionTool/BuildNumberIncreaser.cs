using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace VersionTool
{
    internal class BuildNumberIncreaser
    {
        public static void IncreaseBuildNumber(string versionFilePath)
        {
            Console.WriteLine($"Increasing build number...");

            var doc = XDocument.Load(versionFilePath);

            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            var informationalVersion = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "InformationalVersion").Single();
            TryIncreaseBuildNumber(informationalVersion, TryIncreaseBuildNumberInInformationalVersion);

            var packageVersion = doc.Descendants(ns + "Project").Descendants(ns + "PropertyGroup").Descendants(ns + "Version").Single();
            TryIncreaseBuildNumber(packageVersion, TryIncreaseBuildNumberInPackageVersion);

            doc.Save(versionFilePath);
        }

        private static string TryIncreaseBuildNumber(Regex regex, string value)
        {
            var match = regex.Match(value);
            if (!match.Success)
                return value;

            if (!match.Groups.TryGetValue("build", out var group))
                return value;
            if (!int.TryParse(group.Value, out var buildNumber))
                return value;

            var nextBuildNumber = buildNumber + 1;
            return ReplaceNamedGroup("build", nextBuildNumber.ToString(), match);
        }

        private static string TryIncreaseBuildNumberInInformationalVersion(string value) =>
            TryIncreaseBuildNumber(new Regex(@"^\d+\.\d+\.\d+ \(\d+\.\d+\.\d+ \w+ build (?<build>\d+)\)$"), value);

        private static string TryIncreaseBuildNumberInPackageVersion(string value) =>
            TryIncreaseBuildNumber(new Regex(@"^\d+\.\d+\.\d+-\w+\.(?<build>\d+)$"), value);

        private static string ReplaceNamedGroup(string groupName, string replacement, Match m)
        {
            string capture = m.Value;
            capture = capture.Remove(m.Groups[groupName].Index - m.Index, m.Groups[groupName].Length);
            capture = capture.Insert(m.Groups[groupName].Index - m.Index, replacement);
            return capture;
        }

        private static void TryIncreaseBuildNumber(XElement informationalVersion, Func<string, string> increaser)
        {
            var oldInformationalVersionValue = informationalVersion.Value;
            informationalVersion.Value = increaser(informationalVersion.Value);
            if (oldInformationalVersionValue != informationalVersion.Value)
                Console.WriteLine($"Changing from {oldInformationalVersionValue} to {informationalVersion.Value}");
        }
    }
}