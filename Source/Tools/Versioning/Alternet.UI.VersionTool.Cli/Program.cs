using Alternet.UI.Versioning;
using CommandLine;
using System;
using System.IO;

namespace VersionTool.Cli
{
    internal class Program
    {
        [Verb("set-build-number", HelpText = "Set build number.")]
        class SetBuildNumberOptions
        {
            public SetBuildNumberOptions(int buildNumber)
            {
                BuildNumber = buildNumber;
            }

            [Value(0, Required = true, HelpText = "Build number")]
            public int BuildNumber { get; }
        }

        [Verb("append-version-suffix", HelpText = "Append version suffix.")]
        class AppendVersionSuffixOptions
        {
            public AppendVersionSuffixOptions(string filePath)
            {
                FilePath = filePath;
            }

            [Value(0, Required = true, HelpText = "File path.")]
            public string FilePath { get; }
        }

        private static int Main(string[] args)
        {
            try
            {
                CommandLine.Parser.Default.ParseArguments<SetBuildNumberOptions, AppendVersionSuffixOptions>(args)
                  .MapResult(
                    (SetBuildNumberOptions o) => { BuildNumberSetter.SetBuildNumber(LocateRepository(), o.BuildNumber); return 0; },
                    (AppendVersionSuffixOptions o) => { FileSuffixAppender.AppendVersionSuffix(LocateRepository(), o.FilePath); return 0; },
                    errors => 0);
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                return 1;
            }

            return 0;
        }

        private static Repository LocateRepository()
        {
            var repoRoot = Path.Combine(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? throw new Exception(),
                "../../../../../../../");

            return new Repository(repoRoot);
        }
    }
}