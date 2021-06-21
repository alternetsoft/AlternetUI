// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class GroupBox : Control
    {
        public GroupBox()
        {
            SetNativePointer(NativeApi.GroupBox_Create());
        }
        
        public GroupBox(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public string? Title
        {
            get
            {
                CheckDisposed();
                return NativeApi.GroupBox_GetTitle(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.GroupBox_SetTitle(NativePointer, value);
            }
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GroupBox_Create();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string? GroupBox_GetTitle(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void GroupBox_SetTitle(IntPtr obj, string? value);
            
        }
    }
}
