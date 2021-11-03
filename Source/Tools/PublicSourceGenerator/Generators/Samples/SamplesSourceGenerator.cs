using Alternet.UI.Versioning;
using System;
using System.IO;
using System.Linq;

namespace Alternet.UI.PublicSourceGenerator.Generators.Samples
{
    static class SamplesSourceGenerator
    {
        public static int Generate(Repository repository, ProductVersion productVersion, int buildNumber, string targetFilePath)
        {
            try
            {
                using (var tempDirectory = new TempDirectory("Samples"))
                {
                    var samplesRootRelativePath = @"Source\Samples";
                    var samplesRootFullPath = Path.GetFullPath(Path.Combine(repository.RootPath, samplesRootRelativePath));
                    var sampleDirectoryNames = Directory.GetDirectories(samplesRootFullPath, "*Sample").Select(x => Path.GetFileName(x)!);

                    foreach (var sampleDirectoryName in sampleDirectoryNames)
                    {
                        SourceDirectoryCopier.CopyDirectory(
                            repoRootDirectory: repository.RootPath,
                            tempDirectory: tempDirectory.Path,
                            sourceName: Path.Combine(samplesRootRelativePath, sampleDirectoryName),
                            targetName: sampleDirectoryName,
                            ignoredDirectores: new[] { "build", "Testing" });

                        var projectFiles = Directory.GetFiles(Path.Combine(tempDirectory.Path, sampleDirectoryName), "*Sample.csproj");
                        PatchSampleProjectFile(projectFiles.Single(), productVersion.GetPackageVersion(buildNumber));
                    }

                    File.Copy(
                        Path.Combine(repository.RootPath, @"Publish\PublicFiles\Samples\Alternet.UI.Samples.sln"),
                        Path.Combine(tempDirectory.Path, "Alternet.UI.Samples.sln"));

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

        static void PatchSampleProjectFile(string csprojPath, string version)
        {
            var csproj = new CsprojFile(csprojPath);
            csproj.AddPackageReference("Alternet.UI", version);
            
            csproj.RemoveProjectReference("Alternet.UI.csproj");
            
            csproj.Save();
        }
    }
}
