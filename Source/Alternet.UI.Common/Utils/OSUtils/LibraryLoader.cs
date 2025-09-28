#nullable disable

/*

This code is based on SkiaSharp's LibraryLoader.
We thank the SkiaSharp team for making it available under the MIT license.
We have modified it to fit our needs.

*/

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Alternet.UI
{
    /// <summary>
    /// Provides helper methods to locate, load, resolve symbols and
    /// unload native libraries across supported platforms.
    /// </summary>
    public static class LibraryLoader
    {
        static LibraryLoader()
        {
        }

        /// <summary>
        /// Gets the platform-specific native library file extension
        /// (for example, <c>".dll"</c>, <c>".so"</c>, or <c>".dylib"</c>).
        /// </summary>
        public static string Extension => OSUtils.GetLibraryExtension();

        /// <summary>
        /// Determines the best path to the specified native library by probing common
        /// locations relative to the managed assembly,
        /// the current directory, and application domain paths. If no local file is found,
        /// returns the library name with platform extension.
        /// </summary>
        /// <typeparam name="T">A type from the assembly to use as a reference point when
        /// probing alongside the managed assembly.</typeparam>
        /// <param name="libraryName">The base name of the native library (may or may not
        /// include an extension).</param>
        /// <returns>
        /// A full path to the native library file when found, or the library name
        /// with the platform extension to allow the OS loader
        /// to resolve it from PATH or default locations.
        /// </returns>
        public static string GetLibraryPath<T>(string libraryName)
        {
            var arch = RuntimeInformation.ProcessArchitecture.ToString().ToLowerInvariant();

            var libWithExt = libraryName;
            if (!libraryName.EndsWith(Extension, StringComparison.OrdinalIgnoreCase))
                libWithExt += Extension;

            // 1. try alongside managed assembly
            var path = typeof(T).Assembly.Location;
            if (!string.IsNullOrEmpty(path))
            {
                path = Path.GetDirectoryName(path);
                if (CheckLibraryPath(path, arch, libWithExt, out var localLib))
                    return localLib;
            }

            // 2. try current directory
            if (CheckLibraryPath(Directory.GetCurrentDirectory(), arch, libWithExt, out var lib))
                return lib;

            // 3. try app domain
            try
            {
                if (AppDomain.CurrentDomain is AppDomain domain)
                {
                    // 3.1 RelativeSearchPath
                    if (CheckLibraryPath(domain.RelativeSearchPath, arch, libWithExt, out lib))
                        return lib;

                    // 3.2 BaseDirectory
                    if (CheckLibraryPath(domain.BaseDirectory, arch, libWithExt, out lib))
                        return lib;
                }
            }
            catch
            {
                // no-op as there may not be any domain or path
            }

            // 4. use PATH or default loading mechanism
            return libWithExt;
        }

        /// <summary>
        /// Checks whether the specified library file exists in the given root path,
        /// trying platform-specific and architecture folders first.
        /// </summary>
        /// <param name="root">The base directory to search. May be <c>null</c> or empty.</param>
        /// <param name="arch">The process architecture folder name (for example, "x64", "arm").</param>
        /// <param name="libWithExt">The library file name including extension.</param>
        /// <param name="foundPath">When the method returns <c>true</c>, contains
        /// the full path to the discovered library file; otherwise <c>null</c>.</param>
        /// <returns><c>true</c> if the library file was found in the given root path;
        /// otherwise <c>false</c>.</returns>
        public static bool CheckLibraryPath(string root, string arch, string libWithExt, out string foundPath)
        {
            if (!string.IsNullOrEmpty(root))
            {
                // a. in specific platform sub dir
                var linuxFlavor = LinuxUtils.GetLinuxFlavor();

                if (!string.IsNullOrEmpty(linuxFlavor))
                {
                    var muslLib = Path.Combine(root, linuxFlavor + "-" + arch, libWithExt);
                    if (File.Exists(muslLib))
                    {
                        foundPath = muslLib;
                        return true;
                    }
                }

                // b. in generic platform sub dir
                var searchLib = Path.Combine(root, arch, libWithExt);
                if (File.Exists(searchLib))
                {
                    foundPath = searchLib;
                    return true;
                }

                // c. in root
                searchLib = Path.Combine(root, libWithExt);
                if (File.Exists(searchLib))
                {
                    foundPath = searchLib;
                    return true;
                }
            }

            // d. nothing
            foundPath = null;
            return false;
        }

        /// <summary>
        /// Attempts to load a local library and returns a handle to the loaded library.
        /// </summary>
        /// <remarks>The method resolves the library path based on the specified type
        /// <typeparamref name="T"/> and the provided library name.
        /// Ensure that the library exists at the resolved path and is
        /// accessible by the application.</remarks>
        /// <typeparam name="T">A type used to determine the context for locating
        /// the library path.</typeparam>
        /// <param name="libraryName">The name of the library to load.
        /// This cannot be null or empty.</param>
        /// <returns>A handle to the loaded library if the operation is successful; otherwise,
        /// <see cref="IntPtr.Zero"/>.</returns>
        public static IntPtr TryLoadLocalLibrary<T>(string libraryName)
        {
            var libraryPath = GetLibraryPath<T>(libraryName);
            var handle = LoadLibrary(libraryPath);
            return handle;
        }

        /// <summary>
        /// Loads a native library by probing common locations and returns a handle to the loaded library.
        /// </summary>
        /// <typeparam name="T">A type from the assembly used to probe locations
        /// alongside the managed assembly.</typeparam>
        /// <param name="libraryName">The base name of the native library
        /// (may or may not include an extension).</param>
        /// <returns>An <see cref="IntPtr"/> representing the native library handle.</returns>
        /// <exception cref="DllNotFoundException">Thrown when the library
        /// cannot be loaded by the platform loader.</exception>
        public static IntPtr LoadLocalLibrary<T>(string libraryName)
        {
            var libraryPath = GetLibraryPath<T>(libraryName);

            var handle = LoadLibrary(libraryPath);
            if (handle == IntPtr.Zero)
                throw new DllNotFoundException($"Unable to load library '{libraryName}'.");

            return handle;
        }

        /// <summary>
        /// Attempts to retrieve a delegate of the specified type for a symbol in a native library.
        /// </summary>
        /// <remarks>This method uses
        /// <see cref="Marshal.GetDelegateForFunctionPointer{TDelegate}(IntPtr)"/>
        /// to create the delegate. Ensure that the
        /// delegate type matches the signature of the native function to avoid runtime errors.</remarks>
        /// <typeparam name="T">The type of the delegate to retrieve.
        /// Must inherit from <see cref="Delegate"/>.</typeparam>
        /// <param name="library">A handle to the native library containing the symbol.</param>
        /// <param name="name">The name of the symbol to locate in the native library.</param>
        /// <returns>An instance of the delegate of type <typeparamref name="T"/>
        /// if the symbol is found; otherwise, <see langword="null"/>.</returns>
        public static T TryGetSymbolDelegate<T>(IntPtr library, string name)
            where T : Delegate
        {
            var symbol = GetSymbol(library, name);
            if (symbol == IntPtr.Zero)
                return null;

            return Marshal.GetDelegateForFunctionPointer<T>(symbol);
        }

        /// <summary>
        /// Retrieves a delegate of the specified type for the given symbol exported by a native library.
        /// </summary>
        /// <typeparam name="T">The delegate type that matches the native function signature.</typeparam>
        /// <param name="library">The handle to the native library.</param>
        /// <param name="name">The exported symbol name to resolve.</param>
        /// <returns>An instance of <typeparamref name="T"/> that can be invoked
        /// to call the native function.</returns>
        /// <exception cref="EntryPointNotFoundException">Thrown when the specified
        /// symbol cannot be found in the library.</exception>
        public static T GetSymbolDelegate<T>(IntPtr library, string name)
            where T : Delegate
        {
            var symbol = GetSymbol(library, name);
            if (symbol == IntPtr.Zero)
                throw new EntryPointNotFoundException($"Unable to load symbol '{name}'.");

            return Marshal.GetDelegateForFunctionPointer<T>(symbol);
        }

        /// <summary>
        /// Loads the specified native library using the platform-specific loader.
        /// </summary>
        /// <param name="libraryName">The library file name or full path.</param>
        /// <returns>A handle to the loaded native library, or <c>IntPtr.Zero</c> on failure.</returns>
        public static IntPtr LoadLibrary(string libraryName)
        {
            if (string.IsNullOrEmpty(libraryName))
                return IntPtr.Zero;

            IntPtr handle;

            if (App.IsWindowsOS)
            {
                handle = Win32.LoadLibrary(libraryName);
                return handle;
            }

            if (App.IsLinuxOS)
            {
                handle = Linux.dlopen(libraryName);
                return handle;
            }

            if (App.IsMacOS)
            {
                handle = Mac.dlopen(libraryName);
                return handle;
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Resolves the address of an exported symbol from a native library handle.
        /// </summary>
        /// <param name="library">The native library handle.</param>
        /// <param name="symbolName">The exported symbol name.</param>
        /// <returns>The pointer to the symbol, or <c>IntPtr.Zero</c>
        /// if the symbol cannot be found.</returns>
        public static IntPtr GetSymbol(IntPtr library, string symbolName)
        {
            if (string.IsNullOrEmpty(symbolName))
                return IntPtr.Zero;

            IntPtr handle;
            if (App.IsWindowsOS)
            {
                handle = Win32.GetProcAddress(library, symbolName);
                return handle;
            }

            if (App.IsLinuxOS)
            {
                handle = Linux.dlsym(library, symbolName);
                return handle;
            }

            if (App.IsMacOS)
            {
                handle = Mac.dlsym(library, symbolName);
                return handle;
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Releases the specified dynamically loaded library, freeing associated resources.
        /// </summary>
        /// <remarks>This method determines the operating system at runtime and invokes
        /// the appropriate
        /// platform-specific library release function. Ensure that the handle passed
        /// to this method was obtained from a
        /// compatible library loading mechanism.</remarks>
        /// <param name="library">A handle to the library to be freed.
        /// Must not be <see cref="IntPtr.Zero"/>.</param>
        /// <returns><see langword="true"/> if the library was successfully released
        /// or if the handle is <see cref="IntPtr.Zero"/>; otherwise, <see langword="false"/>.</returns>
        public static bool FreeLibrary(IntPtr library)
        {
            if (library == IntPtr.Zero)
                return true;

            if (App.IsWindowsOS)
            {
                Win32.FreeLibrary(library);
                return true;
            }

            if (App.IsLinuxOS)
            {
                Linux.dlclose(library);
                return true;
            }

            if (App.IsMacOS)
            {
                Mac.dlclose(library);
                return true;
            }

            return false;
        }

#pragma warning disable
        private static class Mac
        {
            private const string SystemLibrary = "/usr/lib/libSystem.dylib";

            private const int RTLD_LAZY = 1;
            private const int RTLD_NOW = 2;

            public static IntPtr dlopen(string path, bool lazy = true) =>
                dlopen(path, lazy ? RTLD_LAZY : RTLD_NOW);

            [DllImport(SystemLibrary)]
            public static extern IntPtr dlopen(string path, int mode);

            [DllImport(SystemLibrary)]
            public static extern IntPtr dlsym(IntPtr handle, string symbol);

            [DllImport(SystemLibrary)]
            public static extern void dlclose(IntPtr handle);
        }

        private static class Linux
        {
            private const string SystemLibrary = "libdl.so";
            private const string SystemLibrary2 = "libdl.so.2"; // newer Linux distros use this

            private const int RTLD_LAZY = 1;
            private const int RTLD_NOW = 2;
            private const int RTLD_DEEPBIND = 8;

            private static bool UseSystemLibrary2 = true;

            public static IntPtr dlopen(string path, bool lazy = true)
            {
                try
                {
                    return dlopen2(path, (lazy ? RTLD_LAZY : RTLD_NOW) | RTLD_DEEPBIND);
                }
                catch (DllNotFoundException)
                {
                    UseSystemLibrary2 = false;
                    return dlopen1(path, (lazy ? RTLD_LAZY : RTLD_NOW) | RTLD_DEEPBIND);
                }
            }

            public static IntPtr dlsym(IntPtr handle, string symbol)
            {
                return UseSystemLibrary2 ? dlsym2(handle, symbol) : dlsym1(handle, symbol);
            }

            public static void dlclose(IntPtr handle)
            {
                if (UseSystemLibrary2)
                    dlclose2(handle);
                else
                    dlclose1(handle);
            }

            [DllImport(SystemLibrary, EntryPoint = "dlopen")]
            private static extern IntPtr dlopen1(string path, int mode);

            [DllImport(SystemLibrary, EntryPoint = "dlsym")]
            private static extern IntPtr dlsym1(IntPtr handle, string symbol);

            [DllImport(SystemLibrary, EntryPoint = "dlclose")]
            private static extern void dlclose1(IntPtr handle);

            [DllImport(SystemLibrary2, EntryPoint = "dlopen")]
            private static extern IntPtr dlopen2(string path, int mode);

            [DllImport(SystemLibrary2, EntryPoint = "dlsym")]
            private static extern IntPtr dlsym2(IntPtr handle, string symbol);

            [DllImport(SystemLibrary2, EntryPoint = "dlclose")]
            private static extern void dlclose2(IntPtr handle);
        }

        private static class Win32
        {
            private const string SystemLibrary = "Kernel32.dll";

            [DllImport(SystemLibrary, SetLastError = true, CharSet = CharSet.Ansi)]
            public static extern IntPtr LoadLibrary(string lpFileName);

            [DllImport(SystemLibrary, SetLastError = true, CharSet = CharSet.Ansi)]
            public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

            [DllImport(SystemLibrary, SetLastError = true, CharSet = CharSet.Ansi)]
            public static extern void FreeLibrary(IntPtr hModule);
        }
#pragma warning restore
    }
}
