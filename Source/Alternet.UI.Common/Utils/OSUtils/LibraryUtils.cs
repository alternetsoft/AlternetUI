using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides utility methods and functionality for working with native libraries.
    /// </summary>
    /// <remarks>This static class serves as a collection of helper methods and utilities designed to
    /// simplify common operations related to native libraries.
    /// All methods are static and can be accessed without instantiating the class.</remarks>
    public static class LibraryUtils
    {
        /// <summary>
        /// Determines whether a native library with the specified name is currently loaded in the process.
        /// </summary>
        /// <remarks>This method checks all loaded modules in the current process to determine if any
        /// module's name contains the specified library name. The comparison is performed
        /// in a case-insensitive
        /// manner.</remarks>
        /// <param name="libraryName">The name of the library to check for.
        /// The comparison is case-insensitive.</param>
        /// <returns><see langword="true"/> if a native library with the specified
        /// name is loaded; otherwise, <see langword="false"/>.</returns>
        public static bool IsNativeLibraryLoadedMsw(string libraryName)
        {
            libraryName = libraryName.ToLowerInvariant();

            if (string.IsNullOrEmpty(libraryName))
                return false;

            var libraryNameAndExtension = libraryName + OSUtils.GetLibraryExtension();

            var process = Process.GetCurrentProcess();
            foreach (ProcessModule module in process.Modules)
            {
                var s = module.ModuleName.ToLowerInvariant();

                if (s == libraryNameAndExtension)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks whether a native library (DLL) is loaded in the current process (Windows/MSW)
        /// and returns its handle (base address) if found, or IntPtr.Zero if not.
        /// </summary>
        public static IntPtr GetNativeLibraryHandleMsw(string libraryName)
        {
            if (string.IsNullOrEmpty(libraryName))
                return IntPtr.Zero;

            libraryName = libraryName.ToLowerInvariant();

            string libraryNameAndExtension = libraryName + OSUtils.GetLibraryExtension();

            var process = Process.GetCurrentProcess();
            foreach (ProcessModule module in process.Modules)
            {
                var moduleName = module.ModuleName.ToLowerInvariant();

                if (moduleName == libraryNameAndExtension)
                {
                    // BaseAddress is the module handle (HMODULE) for Windows DLLs
                    return module.BaseAddress;
                }
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Determines whether the SkiaSharp native library is currently loaded.
        /// </summary>
        /// <returns><see langword="true"/> if the SkiaSharp native library is loaded;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool IsSkiaSharpLibraryLoadedMsw()
        {
            bool loaded = IsNativeLibraryLoadedMsw("libSkiaSharp");
            return loaded;
        }
    }
}
