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
            var filePath = options.FilePath.TrimEnd('\\', '/');
            var isFile = File.Exists(filePath);
            var isFolder = Directory.Exists(filePath);

            if (!isFile && !isFolder)
                throw new FileNotFoundException(filePath);

            var version = VersionService.GetVersion(repository);
            var suffix = "-" + version.GetPackageVersion(VersionService.GetBuildNumber(repository));

            var newName = Path.Combine(
                Path.GetDirectoryName(filePath) ?? throw new Exception(),
                Path.GetFileNameWithoutExtension(filePath) + suffix + Path.GetExtension(filePath));

            if (isFile)
            {
                if(File.Exists(newName))
                    File.Delete(newName);
                File.Move(filePath, newName);
            }
            else
            if (isFolder)
            {
                if (Directory.Exists(newName))
                    Directory.Delete(newName, true);
                Directory.Move(filePath, newName);
            }
        }
    }
}