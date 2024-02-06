using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

[module: DefaultCharSet(CharSet.Unicode)]

namespace Alternet.UI.Native
{
    [SuppressUnmanagedCodeSecurity]
    internal abstract class NativeApiProvider
    {
        public const string NativeModuleNameNoExt = "Alternet.UI.Pal";

#if NETCOREAPP
        public const string NativeModuleName = NativeModuleNameNoExt;
#else
        public const string NativeModuleName = $"{NativeModuleNameNoExt}.dll";
#endif

        private static bool initialized;
        private static GCHandle unhandledExceptionCallbackHandle;
        private static GCHandle caughtExceptionCallbackHandle;

        private static string NativeModuleNameWithExt
        {
            get
            {
                var result = NativeModuleNameNoExt;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    result = $"{result}.dll";
                }
                else
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    result = $"{result}.so";
                }
                else
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    result = $"{result}.dylib";
                }

                return result;
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public delegate void PInvokeCallbackActionType();

        public static void Initialize()
        {
            if (!initialized)
            {
#if NETCOREAPP
                NativeLibrary.SetDllImportResolver(
                    typeof(NativeApiProvider).Assembly,
                    ImportResolver);
#else
                WindowsNativeModulesLocator.SetNativeModulesDirectory();
#endif

                Debug.Assert(
                    !unhandledExceptionCallbackHandle.IsAllocated,
                    "NativeApiProvider.Initialize");
                Debug.Assert(
                    !caughtExceptionCallbackHandle.IsAllocated,
                    "NativeApiProvider.Initialize");

                var unhandledExceptionCallbackSink =
                    new NativeExceptionsMarshal.NativeExceptionCallbackType(
                        NativeExceptionsMarshal.OnUnhandledNativeException);
                unhandledExceptionCallbackHandle =
                    GCHandle.Alloc(unhandledExceptionCallbackSink);

                var caughtExceptionCallbackSink =
                    new NativeExceptionsMarshal.NativeExceptionCallbackType(
                        NativeExceptionsMarshal.OnCaughtNativeException);
                caughtExceptionCallbackHandle =
                    GCHandle.Alloc(caughtExceptionCallbackSink);

                SetExceptionCallback(
                    unhandledExceptionCallbackSink,
                    caughtExceptionCallbackSink);

                initialized = true;
            }
        }

#if NETCOREAPP
        private static IntPtr ImportResolver(
            string libraryName,
            Assembly assembly,
            DllImportSearchPath? searchPath)
        {
            IntPtr libHandle = IntPtr.Zero;
            if (libraryName == NativeModuleName)
            {
                var libraryFileName = FileUtils.FindFileRecursiveInAppFolder(NativeModuleNameWithExt);
                if (libraryFileName is null)
                    return NativeLibrary.Load(libraryName);
                var loaded = NativeLibrary.TryLoad(libraryFileName, out libHandle);
                if (!loaded)
                    return NativeLibrary.Load(libraryName);
            }

            return libHandle;
        }
#endif

        [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SetExceptionCallback(
            NativeExceptionsMarshal.NativeExceptionCallbackType
                unhandledExceptionCallback,
            NativeExceptionsMarshal.NativeExceptionCallbackType
                caughtExceptionCallback);

        private class WindowsNativeModulesLocator
        {
            public static void SetNativeModulesDirectory()
            {
/*#if NETCOREAPP*/
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return;
/*#endif*/
                /*
                var assemblyDirectory = Path.GetDirectoryName(
                    new Uri(
                        typeof(NativeApiProvider).Assembly.EscapedCodeBase).LocalPath)!;
                */
                var assemblyDirectory = Path.GetDirectoryName(
                        typeof(NativeApiProvider).Assembly.Location)!;
                var nativeModulesDirectory =
                    Path.Combine(assemblyDirectory, IntPtr.Size == 8 ? "x64" : "x86");
                if (!Directory.Exists(nativeModulesDirectory))
                    return;

                var ok = SetDllDirectory(nativeModulesDirectory);
                if (!ok)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            private static extern bool SetDllDirectory(string path);
        }
    }
}