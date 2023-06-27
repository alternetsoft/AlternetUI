using Alternet.UI.Versioning;
using CommandLine;
using System;
using System.IO;

namespace VersionTool.Cli
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            try
            {
                var repository = LocateRepository();
                CommandLine.Parser.Default.ParseArguments<
                    BuildNumberSetter.Options,
                    VersionFileSuffixAppender.Options,
                    PublicCommitMessageGenerator.Options,
                    VersionStdoutWriter.Options>(args)
                  .MapResult(
                    (BuildNumberSetter.Options o) => { BuildNumberSetter.SetBuildNumber(repository, o); return 0; },
                    (VersionFileSuffixAppender.Options o) => { VersionFileSuffixAppender.AppendVersionSuffix(repository, o); return 0; },
                    (PublicCommitMessageGenerator.Options o) => { PublicCommitMessageGenerator.Generate(repository, o); return 0; },
                    (VersionStdoutWriter.Options o) => { VersionStdoutWriter.Write(repository); return 0; },
                    errors => 0);
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                //throw; // !! Uncomment this in order to debug
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