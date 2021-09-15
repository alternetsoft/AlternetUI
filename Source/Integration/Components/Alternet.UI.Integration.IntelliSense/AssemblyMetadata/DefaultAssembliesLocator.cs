using NuGet.Versioning;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Alternet.UI.Integration.IntelliSense.AssemblyMetadata
{
    public static class DefaultAssembliesLocator
    {
        public static IEnumerable<string> GetAssemblies()
        {
            var path = TryFindAlternetUIAssemblyFilePath();
            if (path != null)
                yield return path;
        }

        private static string TryFindAlternetUIAssemblyFilePath()
        {
            var packageDirectories = NuGetPackagesLocator.GetNugetPackagesDirs();

            foreach (var packageDirectory in packageDirectories)
            {
                var alternetUIDirectory = new DirectoryInfo(Path.Combine(packageDirectory, "alternet.ui"));
                if (!alternetUIDirectory.Exists)
                    continue;

                var versionDirectories = alternetUIDirectory.EnumerateDirectories();
                var latestVersion = versionDirectories.Select(x =>
                {
                    if (!SemanticVersion.TryParse(x.Name, out var version))
                        return (Version: null, Directory: x);
                    return (Version: version, Directory: x);
                }).Where(x => x.Version != null).OrderByDescending(x => x.Version, VersionComparer.VersionReleaseMetadata).FirstOrDefault();

                if (latestVersion.Version == null)
                    continue;

                var libDirectory = new DirectoryInfo(Path.Combine(latestVersion.Directory.FullName, "lib"));
                var frameworkVersionSubdirectory = libDirectory.GetDirectories().FirstOrDefault();
                if (frameworkVersionSubdirectory == null)
                    continue;

                var assemblyPath = Path.Combine(frameworkVersionSubdirectory.FullName, "Alternet.UI.dll");
                if (!File.Exists(assemblyPath))
                    continue;

                return assemblyPath;
            }

            return null;
        }
    }
}