// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>
#nullable enable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class Clipboard : NativeObject
    {
        static Clipboard()
        {
        }
        
        public Clipboard()
        {
            SetNativePointer(NativeApi.Clipboard_Create_());
        }
        
        public Clipboard(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public UnmanagedDataObject GetDataObject()
        {
            CheckDisposed();
            var n = NativeApi.Clipboard_GetDataObject_(NativePointer);
            var m = NativeObject.GetFromNativePointer<UnmanagedDataObject>(n, p => new UnmanagedDataObject(p))!;
            ReleaseNativeObjectPointer(n);
            return m;
        }
        
        public void SetDataObject(UnmanagedDataObject value)
        {
            CheckDisposed();
            NativeApi.Clipboard_SetDataObject_(NativePointer, value.NativePointer);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Clipboard_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Clipboard_GetDataObject_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Clipboard_SetDataObject_(IntPtr obj, IntPtr value);
            
        }
    }
}
