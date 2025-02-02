using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the DotNet platform.
    /// </summary>
    public static class DotNetUtils
    {
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
                if(runtimesResult is null)
                {
                    var (output, _, _) = AppUtils.ExecuteTerminalCommand(
                            $"dotnet {CmdListRuntimes}",
                            null,
                            true,
                            false);
                    runtimesResult = output ?? string.Empty;
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
                    var (output, _, _) = AppUtils.ExecuteTerminalCommand(
                            $"dotnet {CmdListSdks}",
                            null,
                            true,
                            false);
                    sdksResult = output ?? string.Empty;
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

        internal static void LogInstalledRuntimes()
        {
            App.Log(CmdListRuntimesResult);
        }

        internal static void LogInstalledSdks()
        {
            App.Log(CmdListSdksResult);
        }
    }
}