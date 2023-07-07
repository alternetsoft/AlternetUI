using Alternet.UI.Versioning;
using System;
using System.IO;
using System.Linq;

namespace Alternet.UI.PublicSourceGenerator.Generators.Samples
{
    static class SamplesSourceGenerator
    {
        public static int Generate(Repository repository, ProductVersion productVersion, int buildNumber, string targetDirectoryPath)
        {
            try
            {
                if (Directory.Exists(targetDirectoryPath))
                    Directory.Delete(targetDirectoryPath, recursive: true);
                Directory.CreateDirectory(targetDirectoryPath);

                var samplesRootRelativePath = @"Source\Samples";
                var samplesRootFullPath = Path.GetFullPath(Path.Combine(repository.RootPath, samplesRootRelativePath));
                var sampleDirectoryNames = Directory.GetDirectories(samplesRootFullPath, "*Sample").Select(x => Path.GetFileName(x)!);

                foreach (var sampleDirectoryName in sampleDirectoryNames)
                {
                    SourceDirectoryCopier.CopyDirectory(
                        repoRootDirectory: repository.RootPath,
                        tempDirectory: targetDirectoryPath,
                        sourceName: Path.Combine(samplesRootRelativePath, sampleDirectoryName),
                        targetName: sampleDirectoryName,
                        ignoredDirectores: new[] { "build", "Testing" });

                    var projectFiles = Directory.GetFiles(Path.Combine(targetDirectoryPath, sampleDirectoryName), "*Sample.csproj");
                    PatchSampleProjectFile(projectFiles.Single(), productVersion.GetPackageVersion(buildNumber));
                }

                File.Copy(
                    Path.Combine(repository.RootPath, @"Publish\PublicFiles\Samples\Alternet.UI.Examples.sln"),
                    Path.Combine(targetDirectoryPath, "Alternet.UI.Examples.sln"));

                File.Copy(
                    Path.Combine(repository.RootPath, @"Publish\PublicFiles\Samples\License.txt"),
                    Path.Combine(targetDirectoryPath, "License.txt"));

                File.Copy(
                    Path.Combine(repository.RootPath, @"Publish\PublicFiles\Samples\readme.md"),
                    Path.Combine(targetDirectoryPath, "readme.md"));

                Directory.CreateDirectory(Path.Combine(targetDirectoryPath, "CommonData"));
                void CopyCommonData(string filename)
                {
                    File.Copy(
                        Path.Combine(
                            repository.RootPath,
                            @"Source\Samples\CommonData\" + filename),
                        Path.Combine(targetDirectoryPath, @"CommonData\" + filename));
                }
                CopyCommonData("Sample.ico");
                CopyCommonData("TargetFrameworks.props");

                File.Copy(
                    Path.Combine(repository.RootPath, @"Publish\PublicFiles\Samples\.gitignore"),
                    Path.Combine(targetDirectoryPath, ".gitignore"));

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
