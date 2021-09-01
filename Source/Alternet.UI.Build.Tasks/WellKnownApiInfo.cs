using System;

namespace Alternet.UI.Build.Tasks
{
    internal static class WellKnownApiInfo
    {
        private static ApiInfoProvider? provider;

        public static ApiInfoProvider Provider =>
            provider ??= new ApiInfoProvider(
                typeof(WellKnownApiInfo).Assembly.GetManifestResourceStream("WellKnownApiInfo.xml") ?? throw new Exception());
    }
}