using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Alternet.UI.Common.Utils
{
    /// <summary>
    /// Contains static methods and properties related to Linux.
    /// </summary>
    public static class LinuxUtils
    {
        private static bool? isUbuntu;
        private static string? uNameResult;
        private static bool? isAndroid;

        /// <summary>
        /// Indicates whether the current application is running on Android.
        /// </summary>
        public static bool IsAndroid
        {
            get
            {
                if (isAndroid != null) return (bool)isAndroid;
                return Check();

                static bool Check()
                {
                    using var process = new Process();

                    process.StartInfo.FileName = "getprop";
                    process.StartInfo.Arguments = "ro.build.user";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    try
                    {
                        process.Start();
                        var output = process.StandardOutput.ReadToEnd();
                        isAndroid = string.IsNullOrEmpty(output) ? (bool?)false : (bool?)true;
                    }
                    catch(Exception e)
                    {
                        LogUtils.LogExceptionIfDebug(e);
                        isAndroid = false;
                    }

                    return (bool)isAndroid;
                }
            }
        }

        /// <summary>
        /// Gets results of the "uname -s" call.
        /// </summary>
        public static string UnameResult
        {
            get
            {
                if (uNameResult is not null)
                    return uNameResult;

                try
                {
                    Process p = new()
                    {
                        StartInfo =
                        {
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            FileName = "uname",
                            Arguments = "-s",
                        },
                    };

                    p.Start();
                    string uNameResult = p.StandardOutput.ReadToEnd().Trim();

                    return uNameResult;
                }
                catch(Exception e)
                {
                    LogUtils.LogExceptionIfDebug(e);
                    uNameResult = string.Empty;
                    return uNameResult;
                }
            }
        }

        /// <summary>
        /// Indicates whether the current application is running on Ubuntu.
        /// </summary>
        public static bool IsUbuntu
        {
            get
            {
                if (isUbuntu is null)
                {
                    try
                    {
                        var linuxInfo = new System.IO.StreamReader("/etc/os-release").ReadToEnd();
                        isUbuntu = linuxInfo.Contains("Ubuntu");
                    }
                    catch(Exception e)
                    {
                        LogUtils.LogExceptionIfDebug(e);
                        isUbuntu = false;
                    }
                }

                return isUbuntu.Value;
            }
        }

        /// <summary>
        /// Gets whether specific package is installed.
        /// </summary>
        /// <param name="packageName">Package name.</param>
        /// <returns></returns>
        public static bool? IsPackageInstalled(string packageName)
        {
            try
            {
                var process = new System.Diagnostics.Process()
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "dpkg",
                        Arguments = $"-l | grep {packageName}",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    },
                };
                process.Start();
                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(result))
                {
                    return true;
                }

                return false;
            }
            catch(Exception e)
            {
                LogUtils.LogExceptionIfDebug(e);
                return null;
            }
        }
    }
}
