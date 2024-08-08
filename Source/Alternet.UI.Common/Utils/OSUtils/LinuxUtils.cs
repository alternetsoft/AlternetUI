using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Alternet.UI
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
        /// Gets whether specific package is installed using dpkg utility.
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

        /// <summary>
        /// Contains native methods.
        /// </summary>
        public static class NativeMethods
        {
            /// <summary>
            /// Flag "RTLD_NOW" used in dlopen.
            /// </summary>
            /// <remarks>
            /// All necessary relocations shall be performed when the object is first loaded.
            /// This may waste some processing if relocations are performed for functions that are
            /// never referenced.This behavior may be useful for applications that need to know
            /// as soon as an object is loaded that all symbols referenced during execution are available.
            /// Any object loaded by dlopen() that requires relocations against global symbols
            /// can reference the symbols in the original process image file, any objects loaded
            /// at program start-up, from the object itself as well as any other object included
            /// in the same dlopen() invocation, and any objects that were loaded in any dlopen()
            /// invocation and which specified the RTLD_GLOBAL flag.
            /// </remarks>
            public const int RTLD_NOW = 2;

            /// <summary>
            /// Flag "RTLD_LAZY" used in dlopen.
            /// </summary>
            /// <remarks>
            /// Relocations shall be performed at an implementation-defined time, ranging from
            /// the time of the dlopen() call until the first reference to a given symbol occurs.
            /// Specifying RTLD_LAZY should improve performance on implementations supporting
            /// dynamic symbol binding as a process may not reference all of the functions
            /// in any given object. And, for systems supporting dynamic symbol resolution
            /// for normal process execution, this behavior mimics the normal
            /// handling of process execution.
            /// </remarks>
            public const int RTLD_LAZY = 0x1;

            /// <summary>
            /// Flag "RTLD_LOCAL" used in dlopen.
            /// </summary>
            /// <remarks>
            /// The object's symbols shall not be made available for the relocation processing
            /// of any other object.
            /// </remarks>
            public const int RTLD_LOCAL = 0x4;

            /// <summary>
            /// Flag "RTLD_GLOBAL" used in dlopen.
            /// </summary>
            /// <remarks>
            /// The object's symbols shall be made available for the relocation processing of any
            /// other object. In addition, symbol lookup using dlopen(0, mode) and an associated
            /// dlsym() allows objects loaded with this mode to be searched.
            /// </remarks>
            public const int RTLD_GLOBAL = 8;

            /// <summary>
            /// Native method "dlopen", similar to win-api "LoadLibrary".
            /// </summary>
            [DllImport("libdl.so.2")]
            public static extern IntPtr dlopen(string filename, int flags);

            /// <summary>
            /// Native method "dlsym", similar to win-api "GetProcAddress".
            /// </summary>
            [DllImport("libdl.so.2")]
            public static extern IntPtr dlsym(IntPtr handle, string symbol);

            /// <summary>
            /// Native method "dlclose", similar to win-api "FreeLibrary".
            /// </summary>
            [DllImport("libdl.so.2")]
            public static extern int dlclose(IntPtr handle);

            /// <summary>
            /// Native method "dlerror". Returns last error.
            /// </summary>
            [DllImport("libdl.so.2")]
            public static extern IntPtr dlerror();

            /// <summary>
            /// Returns last error description.
            /// </summary>
            /// <returns></returns>
            public static (string, IntPtr) GetLastError()
            {
                var errPtr = dlerror();
                if (errPtr != IntPtr.Zero)
                    return (Marshal.PtrToStringAnsi(errPtr), errPtr);
                return (string.Empty, default);
            }
        }
    }
}
