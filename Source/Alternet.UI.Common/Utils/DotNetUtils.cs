using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the DotNet platform.
    /// </summary>
    public static class DotNetUtils
    {
        /// <summary>
        /// Gets the default dotnet location on Windows.
        /// </summary>
        public const string DefaultLocationWindows = @"C:\Program Files\dotnet";

        /// <summary>
        /// Gets the default dotnet location on macOs.
        /// </summary>
        public const string DefaultLocationMacOs = @"/usr/local/share/dotnet";

        /// <summary>
        /// Gets the default location for dotnet the x64 runtimes on an arm64 Windows.
        /// </summary>
        public const string DefaultLocationWindowsArmX64 = @"C:\Program Files\dotnet\x64";

        /// <summary>
        /// Gets the default location for the dotnet x64 runtimes on an arm64 macOs.
        /// </summary>
        public const string DefaultLocationMacOsArmX64 = @"/usr/local/share/dotnet/x64";

        /// <summary>
        /// Gets the default dotnet location on Ubuntu 22.04
        /// (when installed from packages.microsoft.com)
        /// </summary>
        /// <remarks>
        /// The default location on Linux varies depending on distro and installment method.
        /// </remarks>
        public const string DefaultLocationUbuntuV1 = @"/usr/share/dotnet";

        /// <summary>
        /// Gets the default dotnet location on Ubuntu 22.04
        /// (when installed from Jammy feed).
        /// </summary>
        /// <remarks>
        /// The default location on Linux varies depending on distro and installment method.
        /// </remarks>
        public const string DefaultLocationUbuntuV2 = @"/usr/lib/dotnet";

        /// <summary>
        /// Gets 'DOTNET_ROOT' environment variable name.
        /// </summary>
        public const string EnvironmentVarDotNetRoot = "DOTNET_ROOT";

        /// <summary>
        /// Gets 'DOTNET_ROOT(x86)' environment variable name.
        /// </summary>
        public const string EnvironmentVarDotNetRootX86v1 = "DOTNET_ROOT(x86)";

        /// <summary>
        /// Gets 'DOTNET_ROOT_X86' environment variable name.
        /// </summary>
        public const string EnvironmentVarDotNetRootX86v2 = "DOTNET_ROOT_X86";

        /// <summary>
        /// Gets 'DOTNET_ROOT_X64' environment variable name.
        /// </summary>
        public const string EnvironmentVarDotNetRootX64 = "DOTNET_ROOT_X64";

        /// <summary>
        /// Gets dotnet command which lists installed runtimes.
        /// </summary>
        public const string CmdListRuntimes = "--list-runtimes";

        /// <summary>
        /// Gets dotnet command which lists installed sdks.
        /// </summary>
        public const string CmdListSdks = "--list-sdks";

        /// <summary>
        /// Gets runtime name for Asp applications.
        /// </summary>
        public const string RuntimeNameAsp = "Microsoft.AspNetCore.App";

        /// <summary>
        /// Gets runtime name for MSW desktop applications.
        /// </summary>
        public const string RuntimeNameWinDesktop = "Microsoft.WindowsDesktop.App";

        /// <summary>
        /// Gets runtime name for core applications.
        /// </summary>
        public const string RuntimeNameNetCore = "Microsoft.NETCore.App";

        private static string? runtimesResult;
        private static string? sdksResult;

        /// <summary>
        /// Gets result of the "dotnet --list-runtimes" command.
        /// Command executed only once and result is cached. In order to
        /// reload the result, assign Null to the property.
        /// </summary>
        public static string CmdListRuntimesResult
        {
            get
            {
                if (runtimesResult is null)
                {
                    var result = AppUtils.ExecuteTerminalCommand(
                            $"dotnet {CmdListRuntimes}",
                            null,
                            true,
                            false);
                    runtimesResult = result.Output ?? string.Empty;
                }

                return runtimesResult;
            }

            set
            {
                runtimesResult = value;
            }
        }

        /// <summary>
        /// Gets result of the "dotnet --list-sdks" command.
        /// Command executed only once and result is cached. In order to
        /// reload the result, assign Null to the property.
        /// </summary>
        public static string CmdListSdksResult
        {
            get
            {
                if (sdksResult is null)
                {
                    var result = AppUtils.ExecuteTerminalCommand(
                            $"dotnet {CmdListSdks}",
                            null,
                            true,
                            false);
                    sdksResult = result.Output ?? string.Empty;
                }

                return sdksResult;
            }

            set
            {
                sdksResult = value;
            }
        }

        /// <summary>
        /// Gets whether the specified dotnet runtime is installed.
        /// </summary>
        /// <param name="majorVersion">Major version number of the dotnet runtime.</param>
        /// <returns></returns>
        public static bool IsNetCoreRuntimeInstalled(int majorVersion)
        {
            var result = CmdListRuntimesResult.Contains($"{RuntimeNameNetCore} {majorVersion}.");
            return result;
        }

        /// <summary>
        /// Gets dotnet location from environment variables.
        /// </summary>
        /// <returns></returns>
        public static string? GetLocationFromEnvironmentVars()
        {
            var result = Environment.GetEnvironmentVariable(EnvironmentVarDotNetRoot);

            if (!string.IsNullOrWhiteSpace(result))
                return result;

            if (App.Is64BitOS)
            {
                result = Environment.GetEnvironmentVariable(EnvironmentVarDotNetRootX64);
                return result;
            }
            else
            {
                result = Environment.GetEnvironmentVariable(EnvironmentVarDotNetRootX86v1);
                if (!string.IsNullOrWhiteSpace(result))
                    return result;
                result = Environment.GetEnvironmentVariable(EnvironmentVarDotNetRootX86v2);
                return result;
            }
        }

        /// <summary>
        /// Gets dotnet location using <see cref="Environment.SpecialFolder.Programs"/>.
        /// Returns Null if not found.
        /// </summary>
        /// <returns></returns>
        public static string? GetDefaultLocationUsingSpecialFolder()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            var dotnetPath = Path.Combine(path, "dotnet");

            return FileUtils.ExistingDirOrNull(dotnetPath);
        }

        /// <summary>
        /// Gets dotnet location. Returns value from the environment variables if they are specified;
        /// otherwise returns default dotnet location. Returned value is specific to the OS.
        /// </summary>
        /// <param name="x64onArm">Whether to return x64 runtime on arm OS.</param>
        /// <returns></returns>
        public static string? GetDefaultLocation(bool x64onArm = false)
        {
            string? Ep(string? path)
            {
                return FileUtils.ExistingDirOrNull(path);
            }

            var result = Ep(GetLocationFromEnvironmentVars());
            if (result is not null)
                return result;

            if (App.IsWindowsOS)
            {
                if (App.IsArmOS && x64onArm)
                {
                    result = Ep(DefaultLocationWindowsArmX64);
                }
                else
                {
                    result = Ep(DefaultLocationWindows);
                }

                if (result is not null)
                    return result;
            }

            if (App.IsMacOS)
            {
                if (App.IsArmOS && x64onArm)
                {
                    result = Ep(DefaultLocationMacOsArmX64);
                }
                else
                {
                    result = Ep(DefaultLocationMacOs);
                }

                if (result is not null)
                    return result;
            }

            if (App.IsLinuxOS)
            {
                result = Ep(DefaultLocationUbuntuV1);
                if (result is not null)
                    return result;

                result = Ep(DefaultLocationUbuntuV2);
                if (result is not null)
                    return result;
            }

            result = GetDefaultLocationUsingSpecialFolder();
            if (result is not null)
                return result;

            return CodeGeneratorUtils.GetDotNetPathFromSystemDllPath();
        }

        internal static void LogInstalledRuntimes()
        {
            App.Log(CmdListRuntimesResult);
        }

        internal static void LogInstalledSdk()
        {
            App.Log(CmdListSdksResult);
        }

        internal static void LogEnvironmentVars()
        {
            App.LogNameValue(
                "DOTNET_ROOT",
                Environment.GetEnvironmentVariable(EnvironmentVarDotNetRoot));

            App.LogNameValue(
                "DOTNET_ROOT(x86)",
                Environment.GetEnvironmentVariable(EnvironmentVarDotNetRootX86v1));

            App.LogNameValue(
                "DOTNET_ROOT_X86",
                Environment.GetEnvironmentVariable(EnvironmentVarDotNetRootX86v2));

            App.LogNameValue(
                "DOTNET_ROOT_X64",
                Environment.GetEnvironmentVariable(EnvironmentVarDotNetRootX64));

            App.LogNameValue(
                "DotNetUtils.GetLocationFromEnvironmentVars()",
                GetLocationFromEnvironmentVars());
        }

        internal static void LogDefaultLocation()
        {
            App.LogNameValue(
                "DotNetUtils.GetDefaultLocation()",
                GetDefaultLocation());
        }
    }
}
