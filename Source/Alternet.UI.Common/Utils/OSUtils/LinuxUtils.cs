﻿using System;
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
                    catch (Exception e)
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
                catch (Exception e)
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
                    catch (Exception e)
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
            catch (Exception e)
            {
                LogUtils.LogExceptionIfDebug(e);
                return null;
            }
        }

        /// <summary>
        /// Retrieves a list of loaded native libraries on Linux.
        /// </summary>
        /// <returns>An array of strings containing the paths of the loaded libraries.</returns>
        internal static string[] GetLoadedLibraries()
        {
            List<string> libraries = new();

            int Callback(ref NativeMethods.DlPhdrInfo info, IntPtr size)
            {
                string name = Marshal.PtrToStringAnsi(info.Name);
                if (!string.IsNullOrEmpty(name))
                    libraries.Add(name);
                return 0;
            }

            NativeMethods.dl_iterate_phdr(Callback, IntPtr.Zero);
            return libraries.ToArray();
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

            internal delegate int DlIteratePhdrCallback(ref DlPhdrInfo info, IntPtr size);

            /// <summary>
            /// Gets process Group ID
            /// </summary>
            /// <returns></returns>
            [DllImport("libc")]
            public static extern int getpgrp();

            /// <summary>
            /// Retrieves the process group ID of the specified process.
            /// </summary>
            /// <remarks>This method is a direct wrapper for the <c>getpgid</c> function in the C
            /// standard library. If the specified process does not exist or
            /// the caller lacks the necessary permissions,
            /// the method will return -1.</remarks>
            /// <param name="pid">The process ID of the target process. Specify
            /// <see langword="0"/> to retrieve the process group ID of
            /// the calling process.</param>
            /// <returns>The process group ID of the specified process, or -1
            /// if an error occurs.</returns>
            [DllImport("libc")]
            public static extern int getpgid(int pid);

            /// <summary>
            /// Creates a new process by duplicating the calling process.
            /// </summary>
            /// <remarks>This method is a direct wrapper for the POSIX <c>fork</c> system call.
            /// It creates a child process that is a copy of the calling process,
            /// except for certain differences such as
            /// process ID and resource allocations. <para> The return value distinguishes
            /// the parent process from the
            /// child process:
            /// <list type="bullet">
            /// <item><description>In the parent process, the return value is the
            /// process ID of the child.</description>
            /// </item>
            /// <item><description>In the child process, the return value
            /// is <see langword="0"/>.</description>
            /// </item>
            /// <item><description>If an error occurs, the return value is
            /// <see langword="-1"/>, and <c>errno</c> is set to indicate the error.</description>
            /// </item> </list>
            /// </para></remarks>
            /// <returns>The process ID of the child process in the
            /// parent, <see langword="0"/> in the child process, or <see
            /// langword="-1"/> if an error occurs.</returns>
            [DllImport("libc", SetLastError = true)]
            public static extern int fork();

            /// <summary>
            /// Executes a specified program, replacing the current process with the new program.
            /// </summary>
            /// <remarks>This method replaces the current process image with a new process image
            /// specified by <paramref name="path"/>. It does not return on success, as the
            /// current process is replaced.
            /// On failure, the method returns -1.</remarks>
            /// <param name="path">The path to the executable file to be executed.
            /// This cannot be null or empty.</param>
            /// <param name="argv">An array of arguments to pass to the program.
            /// The first element is typically the program name. This
            /// cannot be null.</param>
            /// <param name="envp">An array of environment variables to set for the
            /// new program. Each element should be in the format
            /// "KEY=VALUE". This can be null to inherit the current process's environment.</param>
            /// <returns>Returns 0 on success. On failure, returns -1 and sets the
            /// last error, which can be retrieved.</returns>
            [DllImport("libc", SetLastError = true)]
            public static extern int execve(string path, string[] argv, string[] envp);

            /// <summary>
            /// Sends a signal to a process or a group of processes.
            /// </summary>
            /// <remarks>This method is a direct wrapper for the POSIX `kill` function.
            /// It is used to send signals to processes for purposes such as
            /// termination or custom signal handling.</remarks>
            /// <param name="pid">The process ID or process group ID to which the signal
            /// is sent.  If positive, the signal is sent to the
            /// process with the specified ID.  If zero, the signal is sent
            /// to all processes in the caller's process
            /// group.  If negative, the signal is sent to all processes
            /// in the process group with the absolute value of
            /// <paramref name="pid"/>.</param>
            /// <param name="sig">The signal to be sent. This is typically
            /// one of the signal constants defined in the operating system.</param>
            /// <returns>Returns 0 on success. On failure, returns -1 and sets the
            /// last error, which can be retrieved.</returns>
            [DllImport("libc", SetLastError = true)]
            public static extern int kill(int pid, int sig);

            /// <summary>
            /// Makes the process a session leader and starts a new process group.
            /// </summary>
            /// <returns></returns>
            [DllImport("libc")]
            public static extern int setsid();

            /// <summary>
            /// Sets process group id. Returns 0 on success.
            /// </summary>
            /// <param name="pid"></param>
            /// <param name="processGroupId"></param>
            /// <returns></returns>
            [DllImport("libc")]
            public static extern int setpgid(int pid, int processGroupId);

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
            public static (string LastErrorText, IntPtr LastError) GetLastError()
            {
                var errPtr = dlerror();
                if (errPtr != IntPtr.Zero)
                    return (Marshal.PtrToStringAnsi(errPtr), errPtr);
                return (string.Empty, default);
            }

            [DllImport("libc.so.6")]
            internal static extern int dl_iterate_phdr(DlIteratePhdrCallback callback, IntPtr data);

            [StructLayout(LayoutKind.Sequential)]
            internal struct DlPhdrInfo
            {
                public IntPtr Addr;
                public IntPtr Name;
            }
        }
    }
}
