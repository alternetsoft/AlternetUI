using System;
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
                    = Path.GetDirectoryName(typeof(OSUtils).Assembly.Location)
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
