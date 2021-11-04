using Alternet.UI.Versioning;
using CommandLine;
using System;

namespace VersionTool.Cli
{
    internal static class VersionStdoutWriter
    {
        public static void Write(Repository repository)
        {
            var version = VersionService.GetVersion(repository).GetPackageVersion(VersionService.GetBuildNumber(repository));
            Console.Write(version);
        }

        [Verb("get-version", HelpText = "Write the current version to stdout.")]
        public class Options
        {
        }
    }
}