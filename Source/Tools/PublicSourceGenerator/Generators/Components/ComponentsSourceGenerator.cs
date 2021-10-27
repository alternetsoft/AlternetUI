using System;
using System.IO;
using System.IO.Compression;

namespace Alternet.UI.PublicSourceGenerator.Generators.Components
{
    static class ComponentsSourceGenerator
    {
        public static int Generate(ComponentsSourceGeneratorOptions options)
        {
            try
            {
                using (var tempDirectory = new TempDirectory("Components"))
                {
                    var tempDirectoryPath = tempDirectory.Path;

                    SourceDirectoryCopier.CopyDirectory(options.RepoRootDirectory, tempDirectoryPath, "Source\\Keys", "Keys");
                    SourceDirectoryCopier.CopyDirectory(options.RepoRootDirectory, tempDirectoryPath, "Source\\Alternet.UI", "Alternet.UI");

                    PatchPackageReference(
                        Path.Combine(tempDirectoryPath, "Alternet.UI\\Alternet.UI.csproj"),
                        "Alternet.UI.Pal",
                        options.PackagesVersion);

                    tempDirectory.Pack(options.TargetFilePath);
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