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

        internal const string NativeModuleName = $"{NativeModuleNameNoExt}.dll";

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
                var myType = typeof(NativeApiProvider);

                if (!AssemblyUtils.SetMyDllImportResolver(myType, nameof(ImportResolver)))
                    return;

                Debug.Assert(
                    !unhandledExceptionCallbackHandle.IsAllocated,
                    "NativeApiProvider.Initialize");
                Debug.Assert(
                    !caughtExceptionCallbackHandle.IsAllocated,
                    "NativeApiProvider.Initialize");

                try
                {
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
                }
                catch (Exception e)
                {
                    LogCriticalException(e);
                    throw;
                }

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

        internal static IntPtr ImportResolver(
            string libraryName,
            Assembly assembly,
            DllImportSearchPath? searchPath)
        {
            if (libraryName == NativeModuleName && libHandle != default)
                return libHandle;

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
                        libraryName = NativeModuleNameWithExt;

                        var libraryFileName = OSUtils.FindNativeDll(NativeModuleNameWithExt);

                        if (debugResolver)
                        {
                            LogUtils.LogNameValueToFile("FindNativeDll", libraryFileName);
                        }

                        if (libraryFileName is null)
                        {
                            libHandle = AssemblyUtils.NativeLibraryLoad(libraryName, assembly, searchPath);
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
                                libHandle = AssemblyUtils.NativeLibraryLoad(libraryName, assembly, searchPath);
                            }
                        }
                    }

                    result = libHandle;
                }
                else
                    result = AssemblyUtils.NativeLibraryLoad(libraryName);

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
                    handle = LinuxUtils.NativeMethods.dlopen(
                            libraryPath,
                            LinuxUtils.NativeMethods.RTLD_NOW);
                    result = handle != default;
                }
                else
                    result = AssemblyUtils.NativeLibraryTryLoad(libraryPath, out handle);

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
    }
}