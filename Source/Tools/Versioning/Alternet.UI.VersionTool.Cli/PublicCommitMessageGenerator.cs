using Alternet.UI.Versioning;
using CommandLine;
using System;
using System.IO;
using System.Text;

namespace VersionTool.Cli
{
    internal static class PublicCommitMessageGenerator
    {
        [Verb("generate-public-commit-message", HelpText = "Generate public commit message.")]
        public class Options
        {
            public Options(string filePath)
            {
                FilePath = filePath;
            }

            [Value(0, Required = true, HelpText = "File path.")]
            public string FilePath { get; }
        }

        public static void Generate(Repository repository, Options options)
        {
            var version = VersionService.GetVersion(repository).GetPackageVersion(VersionService.GetBuildNumber(repository));

            var sb = new StringBuilder();
            sb.AppendLine("Source code release for version " + version);
            sb.AppendLine();
            sb.AppendLine();
            sb.Append("on-behalf-of: @alternetsoft <yevgeni.zolotko@alternetsoft.com>");

            File.WriteAllText(options.FilePath, sb.ToString());
        }
    }
}