using System;

namespace Alternet.UI.Build.Tasks
{
    internal static class WellKnownApiInfo
    {
        public static ApiInfoProvider Provider { get; } =
            new ApiInfoProvider(
                typeof(WellKnownApiInfo).Assembly.GetManifestResourceStream("WellKnownApiInfo.xml") ?? throw new Exception());
    }
}