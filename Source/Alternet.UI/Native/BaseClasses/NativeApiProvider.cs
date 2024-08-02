using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

using Alternet.UI.Integration;

[module: DefaultCharSet(CharSet.Unicode)]

namespace Alternet.UI.Native
{
    [SuppressUnmanagedCodeSecurity]
    internal abstract class NativeApiProvider
    {
        public static bool DebugImportResolver = DebugUtils.IsDebugDefined;

        internal const string NativeModuleNameNoExt = "Alternet.UI.Pal";

#if NETCOREAPP
        internal const string NativeModuleName = NativeModuleNameNoExt;
        internal static IntPtr libHandle = default;
#else
        internal const string NativeModuleName = $"{NativeModuleNameNoExt}.dll";
#endif

        private static bool initialized;
        private static GCHandle unhandledExceptionCallbackHandle;
        private static GCHandle caughtExceptionCallbackHandle;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        internal delegate void PInvokeCallbackActionType();

        internal static string NativeModuleNameWithExt
        {
            get
            {
                var result = NativeModuleNameNoExt;

                if (App.IsWindowsOS)
                {
                    result = $"{result}.dll";
                }
                else
                if (App.IsLinuxOS)
                {
                    result = $"{result}.so";
                }
                else
                if (App.IsMacOS)
                {
                    result = $"{result}.dylib";
                }

                return result;
            }
        }

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
            var debugResolver = DebugImportResolver && libHandle == default;

            if (debugResolver)
            {
                LogUtils.LogBeginSectionToFile("ImportResolver");
                LogUtils.LogNameValueToFile("libraryName", libraryName);
                LogUtils.LogNameValueToFile("assembly", assembly);
                LogUtils.LogNameValueToFile("searchPath", searchPath);
                LogUtils.LogNameValueToFile("NativeModuleName", NativeModuleName);
                LogUtils.LogNameValueToFile("NativeModuleNameWithExt", NativeModuleNameWithExt);                
            }

            IntPtr result = default;

            if (libraryName == NativeModuleName)
            {
                if(libHandle == default)
                {
                    var libraryFileName =
                        FileUtils.FindFileRecursiveInAppFolder(NativeModuleNameWithExt);

                    if (debugResolver)
                    {
                        LogUtils.LogNameValueToFile("FindFileRecursiveInAppFolder", libraryFileName);
                    }

                    if (libraryFileName is null)
                    {
                        libHandle = NativeLibrary.Load(libraryName);
                    }
                    else
                    {
                        var loaded = NativeLibrary.TryLoad(libraryFileName, out libHandle);

                        if (debugResolver)
                        {
                            LogUtils.LogNameValueToFile("NativeLibrary.TryLoad libHandle", libHandle);
                            LogUtils.LogNameValueToFile("NativeLibrary.TryLoad loaded", loaded);
                        }

                        if (!loaded)
                        {
                            libHandle = NativeLibrary.Load(libraryName);
                        }
                    }
                }

                result = libHandle;
            }
            else
                result = NativeLibrary.Load(libraryName);

            if (debugResolver)
            {
                LogUtils.LogEndSectionToFile();
            }

            return result;
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
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return;
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