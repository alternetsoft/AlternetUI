// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class TabPage : Control
    {
        public TabPage()
        {
            SetNativePointer(NativeApi.TabPage_Create());
        }
        
        public TabPage(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public string? Title
        {
            get
            {
                CheckDisposed();
                return NativeApi.TabPage_GetTitle(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TabPage_SetTitle(NativePointer, value);
            }
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr TabPage_Create();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string? TabPage_GetTitle(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TabPage_SetTitle(IntPtr obj, string? value);
            
        }
    }
}
