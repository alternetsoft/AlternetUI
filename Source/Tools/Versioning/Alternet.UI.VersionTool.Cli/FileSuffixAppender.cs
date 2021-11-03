using Alternet.UI.Versioning;
using System;
using System.IO;

namespace VersionTool.Cli
{
    internal static class FileSuffixAppender
    {
        public static void AppendVersionSuffix(Repository repository, string filePath)
        {
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