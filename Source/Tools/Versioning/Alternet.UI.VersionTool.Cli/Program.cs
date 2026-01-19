using Alternet.UI.Versioning;
using CommandLine;
using System;
using System.IO;

using Alternet.UI;
using System.Diagnostics;

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
                    (BuildNumberSetter.Options o) =>
                    {
                        BuildNumberSetter.SetBuildNumber(repository, o);
                        return 0;
                    },
                    (VersionFileSuffixAppender.Options o) =>
                    {
                        VersionFileSuffixAppender.AppendVersionSuffix(repository, o);
                        return 0;
                    },
                    (PublicCommitMessageGenerator.Options o) =>
                    {
                        PublicCommitMessageGenerator.Generate(repository, o); return 0;
                    },
                    (VersionStdoutWriter.Options o) => {
                        VersionStdoutWriter.Write(repository);
                        return 0;
                    },
                    errors => 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Debug.WriteLine(e.ToString());

                // throw; // !! Uncomment this in order to debug
                return 1;
            }

            return 0;
        }

        private static Repository LocateRepository()
        {
            var uiFolder = Path.Combine(CommonUtils.GetUIFolder("Tools") ?? string.Empty, "..", "..");
            uiFolder = Path.GetFullPath(uiFolder);
            return new Repository(uiFolder);

            /*
            var asmLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;

            var repoRoot = Path.Combine(
                Path.GetDirectoryName(asmLocation) ?? throw new Exception(),
                "../../../../../../../");

            repoRoot = Path.GetFullPath(repoRoot);
            */
        }
    }
}