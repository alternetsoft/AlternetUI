using Alternet.UI.PublicSourceGenerator.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Alternet.UI.PublicSourceGenerator.Generators
{
    static class SourceDirectoryCopier
    {
        public static void CopyDirectory(
            string repoRootDirectory,
            string tempDirectory,
            string sourceName,
            string targetName,
            string[]? ignoredDirectores = null,
            string[]? ignoredFileEndings = null)
        {
            var effectiveIgnoredFiles = new List<string>(new[] { "launchSettings.json" });

            var effectiveIgnoredDirectories = new List<string>(new[] { "bin", "obj", ".vs" });
            if (ignoredDirectores != null)
                effectiveIgnoredDirectories.AddRange(ignoredDirectores);

            var effectiveIgnoredFileEndings = new List<string>(new[] { ".csproj.user" });
            if (ignoredFileEndings != null)
                effectiveIgnoredFileEndings.AddRange(ignoredFileEndings);

            new DirectoryCopier
            {
                SourceDirectoryPath = Path.Combine(repoRootDirectory, sourceName),
                TargetDirectoryPath = Path.Combine(tempDirectory, targetName),
                DirectoryFilter = d => !effectiveIgnoredDirectories.Any(x => d.Name.Equals(x, StringComparison.OrdinalIgnoreCase)),
                FileFilter = f =>
                    !effectiveIgnoredFileEndings.Any(x => f.Name.EndsWith(x, StringComparison.OrdinalIgnoreCase)) &&
                    !effectiveIgnoredFiles.Any(x => f.Name.Equals(x, StringComparison.OrdinalIgnoreCase))
            }.Run();
        }
    }
}