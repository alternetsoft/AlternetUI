using Alternet.UI.Versioning;
using CommandLine;
using System;
using System.IO;

namespace VersionTool.Cli
{
    internal static class VersionFileSuffixAppender
    {
        [Verb("append-version-suffix", HelpText = "Append version suffix.")]
        public class Options
        {
            public Options(string filePath)
            {
                FilePath = filePath;
            }

            [Value(0, Required = true, HelpText = "File path.")]
            public string FilePath { get; }
        }

        public static void AppendVersionSuffix(Repository repository, Options options)
        {
            var filePath = options.FilePath;
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            var version = VersionService.GetVersion(repository);
            var suffix = "-" + version.GetPackageVersion(VersionService.GetBuildNumber(repository));
            var newName = Path.Combine(
                Path.GetDirectoryName(filePath) ?? throw new Exception(),
                Path.GetFileNameWithoutExtension(filePath) + suffix + Path.GetExtension(filePath));
            File.Move(filePath, newName);
        }
    }
}