using System;

namespace Alternet.UI.Integration.VisualStudio.Utils
{
    internal static class FrameworkInfoUtils
    {
        public static bool IsNetFramework(string targetFrameworkIdentifier) =>
            string.Equals(targetFrameworkIdentifier, ".NETFramework", StringComparison.Ordinal);

        public static bool IsNetCoreApp(string targetFrameworkIdentifier) =>
            string.Equals(targetFrameworkIdentifier, ".NETCoreApp", StringComparison.Ordinal);

        public static bool IsNetStandard(string targetFrameworkIdentifier) =>
            string.Equals(targetFrameworkIdentifier, ".NETStandard", StringComparison.Ordinal);
    }
}
