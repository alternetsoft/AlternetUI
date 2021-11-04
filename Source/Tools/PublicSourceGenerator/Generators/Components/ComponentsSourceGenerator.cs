using Alternet.UI.Versioning;
using System;
using System.IO;
using System.IO.Compression;

namespace Alternet.UI.PublicSourceGenerator.Generators.Components
{
    static class ComponentsSourceGenerator
    {
        public static int Generate(Repository repository, ProductVersion productVersion, int buildNumber, string targetDirectoryPath)
        {
            try
            {
                if (Directory.Exists(targetDirectoryPath))
                    Directory.Delete(targetDirectoryPath);
                Directory.CreateDirectory(targetDirectoryPath);

                SourceDirectoryCopier.CopyDirectory(repository.RootPath, targetDirectoryPath, @"Publish\PublicFiles\Components\Keys", "Keys");
                SourceDirectoryCopier.CopyDirectory(repository.RootPath, targetDirectoryPath, "Source\\Alternet.UI", "Alternet.UI");
                SourceDirectoryCopier.CopyDirectory(repository.RootPath, targetDirectoryPath, @"Source\Version", @"Version");
                SourceDirectoryCopier.CopyDirectory(repository.RootPath, targetDirectoryPath, @"Source\Docs", @"Docs");
                SourceDirectoryCopier.CopyDirectory(repository.RootPath, targetDirectoryPath, @"Source\Licenses", @"Licenses");
                SourceDirectoryCopier.CopyDirectory(
                    repository.RootPath,
                    targetDirectoryPath,
                    @"Source\Alternet.UI.Build.Tasks",
                    @"Alternet.UI.Build.Tasks",
                    ignoredDirectores: new[] { "build" });

                PatchPackageReference(
                    Path.Combine(targetDirectoryPath, "Alternet.UI\\Alternet.UI.csproj"),
                    "Alternet.UI.Pal",
                    productVersion.GetPackageVersion(buildNumber));

                File.Copy(
                    Path.Combine(repository.RootPath, @"Publish\PublicFiles\Components\Alternet.UI.sln"),
                    Path.Combine(targetDirectoryPath, "Alternet.UI.sln"));

                File.Copy(
                    Path.Combine(repository.RootPath, @"Publish\PublicFiles\Components\License.txt"),
                    Path.Combine(targetDirectoryPath, "License.txt"));

                File.Copy(
                    Path.Combine(repository.RootPath, @"Publish\PublicFiles\Components\readme.md"),
                    Path.Combine(targetDirectoryPath, "readme.md"));

                File.Copy(
                    Path.Combine(repository.RootPath, @"Publish\PublicFiles\Components\.gitignore"),
                    Path.Combine(targetDirectoryPath, ".gitignore"));

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 1;
            }
        }

        static void PatchPackageReference(string csprojPath, string packageName, string version)
        {
            var csproj = new CsprojFile(csprojPath);
            csproj.RemovePackageReferenceCondition(packageName);
            csproj.SetPackageReferenceVersion(packageName, version);
            csproj.Save();
        }
    }
}