// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.Security;
namespace Alternet.UI.Native
{
    internal class Panel : Control
    {
        public Panel()
        {
            SetNativePointer(NativeApi.Panel_Create());
        }
        
        public Panel(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Panel_Create();
            
        }
    }
}
