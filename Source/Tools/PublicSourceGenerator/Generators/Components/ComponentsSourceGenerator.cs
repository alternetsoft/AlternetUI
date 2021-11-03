using Alternet.UI.Versioning;
using System;
using System.IO;
using System.IO.Compression;

namespace Alternet.UI.PublicSourceGenerator.Generators.Components
{
    static class ComponentsSourceGenerator
    {
        public static int Generate(Repository repository, ProductVersion productVersion, int buildNumber, string targetFilePath)
        {
            try
            {
                using (var tempDirectory = new TempDirectory("Components"))
                {
                    var tempDirectoryPath = tempDirectory.Path;

                    SourceDirectoryCopier.CopyDirectory(repository.RootPath, tempDirectoryPath, @"Publish\PublicFiles\Components\Keys", "Keys");
                    SourceDirectoryCopier.CopyDirectory(repository.RootPath, tempDirectoryPath, "Source\\Alternet.UI", "Alternet.UI");
                    SourceDirectoryCopier.CopyDirectory(repository.RootPath, tempDirectoryPath, @"Source\Version", @"Version");
                    SourceDirectoryCopier.CopyDirectory(
                        repository.RootPath,
                        tempDirectoryPath,
                        @"Source\Alternet.UI.Build.Tasks",
                        @"Alternet.UI.Build.Tasks",
                        ignoredDirectores: new[] { "build" });

                    PatchPackageReference(
                        Path.Combine(tempDirectoryPath, "Alternet.UI\\Alternet.UI.csproj"),
                        "Alternet.UI.Pal",
                        productVersion.GetPackageVersion(buildNumber));

                    File.Copy(
                        Path.Combine(repository.RootPath, @"Publish\PublicFiles\Components\Alternet.UI.sln"),
                        Path.Combine(tempDirectory.Path, "Alternet.UI.sln"));

                    File.Copy(
                        Path.Combine(repository.RootPath, @"Publish\PublicFiles\Components\License.txt"),
                        Path.Combine(tempDirectory.Path, "License.txt"));

                    File.Copy(
                        Path.Combine(repository.RootPath, @"Publish\PublicFiles\Components\readme.md"),
                        Path.Combine(tempDirectory.Path, "readme.md"));


                    tempDirectory.Pack(targetFilePath);
                }

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