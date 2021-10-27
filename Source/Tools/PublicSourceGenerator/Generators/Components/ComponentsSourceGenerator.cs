using Alternet.UI.Versioning;
using System;
using System.IO;
using System.IO.Compression;

namespace Alternet.UI.PublicSourceGenerator.Generators.Components
{
    static class ComponentsSourceGenerator
    {
        public static int Generate(Repository repository, ProductVersion productVersion, string targetFilePath)
        {
            try
            {
                using (var tempDirectory = new TempDirectory("Components"))
                {
                    var tempDirectoryPath = tempDirectory.Path;

                    SourceDirectoryCopier.CopyDirectory(repository.RootPath, tempDirectoryPath, "Source\\Keys", "Keys");
                    SourceDirectoryCopier.CopyDirectory(repository.RootPath, tempDirectoryPath, "Source\\Alternet.UI", "Alternet.UI");
                    SourceDirectoryCopier.CopyDirectory(repository.RootPath, tempDirectoryPath, @"Source\Mastering\Version", @"Mastering\Version");
                    SourceDirectoryCopier.CopyDirectory(repository.RootPath, tempDirectoryPath, @"Source\Alternet.UI.Build.Tasks\msbuild", @"Alternet.UI.Build.Tasks\msbuild");

                    PatchPackageReference(
                        Path.Combine(tempDirectoryPath, "Alternet.UI\\Alternet.UI.csproj"),
                        "Alternet.UI.Pal",
                        productVersion.GetPackageReferenceVersion());

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