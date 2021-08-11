// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class Font : NativeObject
    {
        public Font()
        {
            SetNativePointer(NativeApi.Font_Create_());
        }
        
        public Font(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public void Initialize(string familyName, float emSize)
        {
            CheckDisposed();
            NativeApi.Font_Initialize_(NativePointer, familyName, emSize);
        }
        
        public void InitializeWithDefaultFont()
        {
            CheckDisposed();
            NativeApi.Font_InitializeWithDefaultFont_(NativePointer);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Font_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Font_Initialize_(IntPtr obj, string familyName, float emSize);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Font_InitializeWithDefaultFont_(IntPtr obj);
            
        }
    }
}
