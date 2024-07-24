using Alternet.UI.Versioning;
using System;
using System.Collections.Generic;
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
                var samplesRootFullPath
                    = Path.GetFullPath(Path.Combine(repository.RootPath, samplesRootRelativePath));
                var sampleDirectoryNames1
                    = Directory.GetDirectories(samplesRootFullPath, "*Sample").Select(x => Path.GetFileName(x)!);
                var sampleDirectoryNames2
                    = Directory.GetDirectories(samplesRootFullPath, "*SampleDll").Select(x => Path.GetFileName(x)!);

                List<string> sampleDirectoryNames = new();
                sampleDirectoryNames.AddRange(sampleDirectoryNames1);
                sampleDirectoryNames.AddRange(sampleDirectoryNames2);

                foreach (var sampleDirectoryName in sampleDirectoryNames)
                {
                    SourceDirectoryCopier.CopyDirectory(
                        repoRootDirectory: repository.RootPath,
                        tempDirectory: targetDirectoryPath,
                        sourceName: Path.Combine(samplesRootRelativePath, sampleDirectoryName),
                        targetName: sampleDirectoryName,
                        ignoredDirectores: new[] { "build", "Testing" });

                    var projectFiles1 = Directory.GetFiles(
                        Path.Combine(targetDirectoryPath, sampleDirectoryName),
                        "*Sample.csproj");
                    var projectFiles2 = Directory.GetFiles(
                        Path.Combine(targetDirectoryPath, sampleDirectoryName),
                        "*SampleDll.csproj");

                    List<string> projectFiles = new();
                    projectFiles.AddRange(projectFiles1);
                    projectFiles.AddRange(projectFiles2);

                    PatchSampleProjectFile(
                        projectFiles.Single(),
                        productVersion.GetPackageVersion(buildNumber));
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

                File.Copy(
                    Path.Combine(repository.RootPath, @"Publish\PublicFiles\Samples\NuGet.config"),
                    Path.Combine(targetDirectoryPath, "NuGet.config"));                

                Directory.CreateDirectory(Path.Combine(targetDirectoryPath, "CommonData"));
                Directory.CreateDirectory(Path.Combine(targetDirectoryPath, "LocalPackages"));

                File.Copy(
                    Path.Combine(repository.RootPath, @"Publish\PublicFiles\Samples\LocalPackages\readme.md"),
                    Path.Combine(targetDirectoryPath, @"LocalPackages\readme.md"));

                var sourceCommonDataPath = Path.Combine(
                            repository.RootPath,
                            @"Source\Samples\CommonData\");

                var destCommonDataPath = Path.Combine(targetDirectoryPath, @"CommonData\");

                void CopyCommonData(string filename)
                {
                    File.Copy(
                        sourceCommonDataPath + filename,
                        destCommonDataPath + filename);
                }

                var commonDataFiles = Directory.GetFiles(sourceCommonDataPath, "*");
                foreach(var commonDataFile in commonDataFiles)
                {
                    var commonDataFileName = Path.GetFileName(commonDataFile);
                    CopyCommonData(commonDataFileName);
                }

                PatchSampleProjectFile(
                    destCommonDataPath + "CommonProject.props",
                    productVersion.GetPackageVersion(buildNumber),
                    false,
                    true);


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

        static void PatchSampleProjectFile(
            string csprojPath,
            string version,
            bool addPackage = true,
            bool debug = false)
        {
            var csproj = new CsprojFile(csprojPath);
            
            csproj.RemoveProjectReference("Alternet.UI.csproj", debug);
            csproj.RemoveProjectReference("Alternet.UI.Interfaces.csproj");
            csproj.RemoveProjectReference("Alternet.UI.Common.csproj");

            if(addPackage)
                csproj.AddPackageReference("Alternet.UI", version);

            csproj.Save();
        }
    }
}
