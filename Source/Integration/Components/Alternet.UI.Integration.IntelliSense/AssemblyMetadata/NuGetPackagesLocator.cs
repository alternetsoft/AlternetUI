using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Alternet.UI.Integration.IntelliSense.AssemblyMetadata
{
    public static class NuGetPackagesLocator
    {
        public static string[] GetNugetPackagesDirs()
        {
            var home = Environment.GetEnvironmentVariable(
#if DESKTOP
                Environment.OSVersion.Platform == PlatformID.Win32NT
#else
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
#endif
                    ? "USERPROFILE"
                    : "HOME");

            var redirectedPath = Environment.GetEnvironmentVariable("NUGET_PACKAGES");

            if (redirectedPath != null)
            {
                return new[] { Path.Combine(home, ".nuget/packages"), redirectedPath };
            }
            else
            {
                return new[] { Path.Combine(home, ".nuget/packages") };
            }
        }
    }
}