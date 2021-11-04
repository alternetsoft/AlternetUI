using Alternet.UI.Versioning;
using CommandLine;

namespace VersionTool.Cli
{
    internal static class BuildNumberSetter
    {
        public static void SetBuildNumber(Repository repository, Options options)
        {
            Alternet.UI.Versioning.BuildNumberSetter.SetBuildNumber(repository, options.BuildNumber);
        }

        [Verb("set-build-number", HelpText = "Set build number.")]
        public class Options
        {
            public Options(int buildNumber)
            {
                BuildNumber = buildNumber;
            }

            [Value(0, Required = true, HelpText = "Build number")]
            public int BuildNumber { get; }
        }
    }
}