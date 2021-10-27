using Alternet.UI.PublicSourceGenerator.Generators.Components;
using Alternet.UI.PublicSourceGenerator.Generators.Samples;
using Alternet.UI.Versioning;
using CommandLine;
using System;
using System.IO;

namespace Alternet.UI.PublicSourceGenerator
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                var repository = LocateRepository();
                var productVersion = VersionService.GetVersion(repository);
                var targetDirectory = Path.Combine(repository.RootPath, "Publish/Packages");

                return CommandLine.Parser.Default.ParseArguments<ComponentsSourceGeneratorOptions, SamplesSourceGeneratorOptions>(args)
                  .MapResult(
                    (ComponentsSourceGeneratorOptions o) => ComponentsSourceGenerator.Generate(
                        repository,
                        productVersion,
                        Path.Combine(targetDirectory, $"PublicSourceCode-{productVersion.GetPackageVersion()}.zip")),
                    (SamplesSourceGeneratorOptions o) => SamplesSourceGenerator.Generate(
                        repository,
                        productVersion,
                        Path.Combine(targetDirectory, $"PublicSamples-{productVersion.GetPackageVersion()}.zip")),
                    errors => 1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 1;
            }
        }

        private static Repository LocateRepository()
        {
            var repoRoot = Path.Combine(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? throw new Exception(),
                "../../../../../../");

            return new Repository(repoRoot);
        }
    }
}