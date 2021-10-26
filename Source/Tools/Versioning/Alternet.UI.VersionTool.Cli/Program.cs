using Alternet.UI.Versioning;
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
                CheckArguments(args, out var buildNumber);
                BuildNumberSetter.SetBuildNumber(LocateRepository(), buildNumber);
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                return 1;
            }

            return 0;
        }

        private static void CheckArguments(string[] args, out int buildNumber)
        {
            if (args.Length != 2 || !args[0].Equals("set-build-number", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Usage: Alternet.UI.VersionTool.Cli.exe set-build-number <build-number>");
            buildNumber = int.Parse(args[1]);
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