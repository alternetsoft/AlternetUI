using CommandLine;

namespace Alternet.UI.PublicSourceGenerator.Generators.Samples
{
    [Verb("samples", HelpText = "Generate samples source.")]
    class SamplesSourceGeneratorOptions
    {
        [Option('r', "repoRoot", Required = true, HelpText = "Repository repo root path.")]
        public string RepoRootDirectory { get; set; } = null!;

        [Option('t', "targetFile", Required = true, HelpText = "Target file path.")]
        public string TargetFilePath { get; set; } = null!;

        [Option('v', "packagesVersion", Required = true, HelpText = "NuGet packages version.")]
        public string PackagesVersion { get; set; } = null!;
    }
}
