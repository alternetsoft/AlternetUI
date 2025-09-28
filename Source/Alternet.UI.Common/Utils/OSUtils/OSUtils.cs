using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the operating system.
    /// </summary>
    public static class OSUtils
    {
        /// <summary>
        /// Gets or sets the file extension override for the native libraries.
        /// </summary>
        /// <remarks>This property allows you to specify a custom file extension to be used by the
        /// native libraries.
        /// If set to <see langword="null"/>, the default file extension will be used.</remarks>
        public static string? NativeLibraryExtensionOverride;

        private static ConcurrentDictionary<string, string?> commandPath { get; } = new();

        /// <summary>
        /// Tries to get the full path for a given command.
        /// </summary>
        /// <param name="command">The command to find the full path for.</param>
        /// <returns>The full path of the command if found; otherwise, null.</returns>
        public static string? TryGetFullPathForCommand(string command)
        {
            if (commandPath.TryGetValue(command, out string? result))
            {
                return result;
            }

            if (App.IsWindowsOS)
            {
                result = FileUtils.GetFullPathUsingWhere(command);
            }
            else
            {
                var result1 = AppUtils.ExecuteTerminalCommand($"which {command}", null, true, false);
                result = result1.Output;
            }

            commandPath[command] = result;
            return result;
        }

        /// <summary>
        /// Tries to find native library in the subfolder of the application.
        /// </summary>
        /// <param name="nativeModuleNameWithExt">Name and extension of the library.</param>
        /// <returns></returns>
        public static string? FindNativeDll(string nativeModuleNameWithExt)
        {
            /*
            runtimes\win-x64\native
            runtimes\win-x86\native
            linux-x64\
            osx-x64
            x64
            x86
            In the same folder
            */

            string GetRuntimesSubFolderPrefix()
            {
                if (App.IsWindowsOS)
                    return "win";
                if (App.IsMacOS)
                    return "osx";
                if (App.IsLinuxOS)
                    return "linux";
                return "unknown";
            }

            string GetRuntimesFolderSuffix(bool withArm = true)
            {
                if (App.IsArmProcess && withArm)
                {
                    var ptrSuffix = IntPtr.Size == 8 ? "arm64" : "arm32";
                    return ptrSuffix;
                }
                else
                {
                    var ptrSuffix = IntPtr.Size == 8 ? "x64" : "x86";
                    return ptrSuffix;
                }
            }

            string GetRuntimesFolder()
            {
                var result = Path.Combine(
                    "runtimes",
                    $"{GetRuntimesSubFolderPrefix()}-{GetRuntimesFolderSuffix()}",
                    "native");
                return result;
            }

            string GetDllPath(string? subFolder)
            {
                var assemblyDirectory
                    = Path.GetDirectoryName(AssemblyUtils.GetLocationSafe(typeof(OSUtils).Assembly))
                    ?? PathUtils.GetAppFolder();
                var moduleName = nativeModuleNameWithExt;
                string result;
                if (subFolder is null)
                    result = Path.Combine(assemblyDirectory, moduleName);
                else
                    result = Path.Combine(assemblyDirectory, subFolder, moduleName);
                return result;
            }

            var testPath = GetDllPath(GetRuntimesFolder());
            if (File.Exists(testPath))
                return testPath;

            testPath = GetDllPath(GetRuntimesFolderSuffix());
            if (File.Exists(testPath))
                return testPath;

            testPath = GetDllPath(null);
            if (File.Exists(testPath))
                return testPath;

            testPath = GetDllPath(GetRuntimesFolderSuffix(false));
            if (File.Exists(testPath))
                return testPath;

            return FileUtils.FindFileRecursiveInAppFolder(nativeModuleNameWithExt);
        }

        /// <summary>
        /// Gets dynamic library extension depending on the current operating system.
        /// </summary>
        /// <returns></returns>
        public static string GetLibraryExtension()
        {
            if (NativeLibraryExtensionOverride is not null)
                return NativeLibraryExtensionOverride;

            if (App.IsWindowsOS)
                return ".dll";
            if (App.IsLinuxOS || App.IsAndroidOS)
                return ".so";
            if (App.IsMacOS || App.IsIOS)
                return ".dylib";
            return ".so";
        }

        /// <summary>
        /// Retrieves a list of loaded native libraries (Windows, Linux, macOs).
        /// </summary>
        /// <returns>An array of strings containing the paths of the loaded libraries.</returns>
        public static string[] GetLoadedLibraries()
        {
            if (App.IsWindowsOS)
            {
                return MswUtils.GetLoadedLibraries();
            }
            else
            if(App.IsLinuxOS)
            {
                return LinuxUtils.GetLoadedLibraries();
            }
            else
            if (App.IsMacOS)
            {
                return MacOsUtils.GetLoadedLibraries();
            }
            else
            {
                return [];
            }
        }

        /// <summary>
        /// Adds an extension to the specified native module name. The extension depends on the
        /// operating system.
        /// </summary>
        /// <param name="nativeModuleNameNoExt">Native module name without an extension.</param>
        /// <returns></returns>
        public static string GetNativeModuleName(string nativeModuleNameNoExt)
        {
            var result = nativeModuleNameNoExt;

            if (App.IsWindowsOS)
            {
                result = $"{result}.dll";
            }
            else
            if (App.IsLinuxOS || App.IsAndroidOS)
            {
                result = $"{result}.so";
            }
            else
            if (App.IsMacOS || App.IsIOS)
            {
                result = $"{result}.dylib";
            }

            return result;
        }

        /// <summary>
        /// Suspends the execution of the current thread for a specified interval.
        /// </summary>
        /// <param name="milliSeconds">Specifies time, in milliseconds, for which
        /// to suspend execution.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sleep(int milliSeconds)
        {
            Thread.Sleep(milliSeconds);
        }

        /// <summary>
        /// Retrieves the low-order word from the specified value.
        /// </summary>
        /// <param name="value">Specifies the value to be converted.</param>
        /// <returns>The return value is the low-order word of the specified value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short LoWord(IntPtr value)
        {
            return (short)value.ToInt32();
        }

        /// <summary>
        /// Retrieves the high-order word from the given value.
        /// </summary>
        /// <param name="value">Specifies the value to be converted.</param>
        /// <returns>The return value is the high-order word of the specified value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short HiWord(IntPtr value)
        {
            return (short)(value.ToInt32() >> 16);
        }
    }
}
