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
        internal const string NativeModuleNameNoExt = "Alternet.UI.Pal";

#if NETCOREAPP
        internal const string NativeModuleName = NativeModuleNameNoExt;
#else
        internal const string NativeModuleName = $"{NativeModuleNameNoExt}.dll";
#endif

        internal static IntPtr libHandle = default;

        private static bool initialized;
        private static GCHandle unhandledExceptionCallbackHandle;
        private static GCHandle caughtExceptionCallbackHandle;

        static NativeApiProvider()
        {
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        internal delegate void PInvokeCallbackActionType();

        internal static string NativeModuleNameWithExt
        {
            get
            {
                return OSUtils.GetNativeModuleName(NativeModuleNameNoExt);
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

        public static void LogCriticalException(Exception? e)
        {
            try
            {
                var s = $"Critical error\n\n";
                s += $"Application: [{CommonUtils.GetAppExePath()}]\n\n";

                s += $"Error loading [{NativeModuleNameWithExt}] library.\n";
                s += $"Exception info logged to [{App.LogFilePath}].\n";
                if (App.IsLinuxOS)
                {
                    s += $"Please run [ldd {NativeModuleNameWithExt}] command " +
                        "in the terminal in order to get the library references.\n";
                    s += "If there are any 'not found' references, you need to install " +
                        "appropriate packages before running this application.\n";
                }

                DialogFactory.ShowCriticalMessage(s, e);
            }
            catch
            {
            }
        }

        internal static bool NativeLibraryTryLoad(string libraryPath, out IntPtr handle)
        {
#if NETCOREAPP
            return NativeLibrary.TryLoad(libraryPath, out handle);
#else
            handle = default;
            return false;
#endif
        }

        internal static IntPtr NativeLibraryLoad(
            string libraryName,
            Assembly assembly,
            DllImportSearchPath? searchPath)
        {
#if NETCOREAPP
            return NativeLibrary.Load(
                libraryName,
                assembly,
                searchPath);
#else
            return default;
#endif
        }

        internal static IntPtr NativeLibraryLoad(string libraryPath)
        {
#if NETCOREAPP
            return NativeLibrary.Load(libraryPath);
#else
            return default;
#endif
        }

        internal static IntPtr ImportResolver(
            string libraryName,
            Assembly assembly,
            DllImportSearchPath? searchPath)
        {
            var debugResolver = DebugUtils.DebugLoading && libHandle == default;

            try
            {
                return Fn();
            }
            catch (Exception e)
            {
                LogCriticalException(e);
                throw;
            }

            IntPtr Fn()
            {
                if (debugResolver)
                {
                    LogUtils.LogBeginSectionToFile("ImportResolver");
                    LogUtils.LogNameValueToFile("libraryName", libraryName);
                    LogUtils.LogNameValueToFile("assembly", assembly);
                    LogUtils.LogNameValueToFile("searchPath", searchPath);
                    LogUtils.LogNameValueToFile("NativeModuleName", NativeModuleName);
                    LogUtils.LogNameValueToFile("NativeModuleNameWithExt", NativeModuleNameWithExt);
                }

                IntPtr result;

                if (libraryName == NativeModuleName)
                {
                    if (libHandle == default)
                    {
                        var libraryFileName = OSUtils.FindNativeDll(NativeModuleNameWithExt);

                        if (debugResolver)
                        {
                            LogUtils.LogNameValueToFile("FindNativeDll", libraryFileName);
                        }

                        if (libraryFileName is null)
                        {
                            libHandle = NativeLibraryLoad(libraryName, assembly, searchPath);
                        }
                        else
                        {
                            var loaded = FnTryLoadLibrary(libraryFileName, out libHandle);

                            if (debugResolver)
                            {
                                LogUtils.LogNameValueToFile(
                                    "NativeLibrary.TryLoad libHandle",
                                    libHandle);
                                LogUtils.LogNameValueToFile("NativeLibrary.TryLoad loaded", loaded);
                            }

                            if (!loaded)
                            {
                                libHandle = NativeLibraryLoad(libraryName, assembly, searchPath);
                            }
                        }
                    }

                    result = libHandle;
                }
                else
                    result = NativeLibraryLoad(libraryName);

                if (debugResolver)
                {
                    LogUtils.LogEndSectionToFile();
                }

                return result;
            }

            bool FnTryLoadLibrary(string libraryPath, out IntPtr handle)
            {
                bool result;

                if (App.IsLinuxOS && DebugUtils.UseDlOpenOnLinux)
                {
                    handle =
                        LinuxUtils.NativeMethods.dlopen(libraryPath, LinuxUtils.NativeMethods.RTLD_NOW);
                    result = handle != default;
                }
                else
                    result = NativeLibraryTryLoad(libraryPath, out handle);

                if (App.IsLinuxOS && debugResolver)
                {
                    if (!result)
                    {
                        try
                        {
                            var errorText = LinuxUtils.NativeMethods.GetLastError();
                            LogUtils.LogNameValueToFile("Error", errorText);
                        }
                        catch
                        {
                        }
                    }
                }
                return result;
            }
        }

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
                if (!App.IsWindowsOS)
                    return;

                var b = DebugUtils.DebugLoading && DebugUtils.IsDebugDefined;

                try
                {
                    if (b)
                        LogUtils.LogBeginSectionToFile("SetNativeModulesDirectory");

                    var libraryFileName = OSUtils.FindNativeDll(NativeModuleNameWithExt);

                    if (b)
                        LogUtils.LogNameValueToFile("libraryFileName", libraryFileName);

                    var nativeModulesDirectory = Path.GetDirectoryName(libraryFileName);
                    if (!Directory.Exists(nativeModulesDirectory))
                        return;

                    if (b)
                        LogUtils.LogNameValueToFile("nativeModulesDirectory", nativeModulesDirectory);

                    var ok = SetDllDirectory(nativeModulesDirectory);

                    if (b)
                        LogUtils.LogNameValueToFile("SetDllDirectory", ok);

                    if (!ok)
                        throw new Win32Exception(Marshal.GetLastWin32Error());

                }
                catch (Exception e)
                {
                    LogCriticalException(e);
                    throw;
                }

                if (DebugUtils.DebugLoading)
                {

                }
            }

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            private static extern bool SetDllDirectory(string path);
        }
    }
}