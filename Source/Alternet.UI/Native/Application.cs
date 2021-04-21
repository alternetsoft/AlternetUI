// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class Application : NativeObject
    {
        public Application()
        {
            SetNativePointer(NativeApi.Application_Create());
        }
        
        public Application(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public void Run(Window window)
        {
            CheckDisposed();
            NativeApi.Application_Run(NativePointer, window.NativePointer);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Application_Create();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Application_Run(IntPtr obj, IntPtr window);
            
        }
    }
}
