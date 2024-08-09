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
        public static bool DebugImportResolver = DebugUtils.IsDebugDefined && false;

        public static bool UseDlOpenOnLinux = false;

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

#if NETCOREAPP
        private static IntPtr ImportResolver(
            string libraryName,
            Assembly assembly,
            DllImportSearchPath? searchPath)
        {
            var debugResolver = DebugImportResolver && libHandle == default;

            try
            {
                return Fn();
            }
            catch (Exception e)
            {
                LogException(e);
                throw;
            }

            void LogException(Exception? e)
            {
                if(e is not null)
                    LogUtils.LogExceptionToFile(e);

                var s = $"\nError loading '{NativeModuleNameWithExt}' library.\n";
                s += $"Exception info logged to '{App.LogFilePath}'.\n";
                if (App.IsLinuxOS)
                {
                    s += $"Please run 'ldd {NativeModuleNameWithExt}' command " +
                        "in the terminal in order to get the library references.\n";
                    s += "If there are any 'not found' references, you need to install " +
                        "appropriate packages before running this application.\n";
                }

                DialogFactory.ShowCriticalMessage(s);
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
                        var libraryFileName =
                            FileUtils.FindFileRecursiveInAppFolder(NativeModuleNameWithExt);

                        if (debugResolver)
                        {
                            LogUtils.LogNameValueToFile("FindFileRecursiveInAppFolder", libraryFileName);
                        }

                        if (libraryFileName is null)
                        {
                            libHandle = NativeLibrary.Load(libraryName, assembly, searchPath);
                        }
                        else
                        {
                            var loaded = FnTryLoadLibrary(libraryFileName, out libHandle);

                            if (debugResolver)
                            {
                                LogUtils.LogNameValueToFile("NativeLibrary.TryLoad libHandle", libHandle);
                                LogUtils.LogNameValueToFile("NativeLibrary.TryLoad loaded", loaded);
                            }

                            if (!loaded)
                            {
                                libHandle = NativeLibrary.Load(libraryName, assembly, searchPath);
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

            bool FnTryLoadLibrary(string libraryPath, out IntPtr handle)
            {
                bool result;

                if (App.IsLinuxOS && UseDlOpenOnLinux)
                {
                    handle =
                        LinuxUtils.NativeMethods.dlopen(libraryPath, LinuxUtils.NativeMethods.RTLD_NOW);
                    result = handle != default;
                }
                else
                    result = NativeLibrary.TryLoad(libraryPath, out handle);

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