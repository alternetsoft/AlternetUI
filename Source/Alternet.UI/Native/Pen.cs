// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class Pen : NativeObject
    {
        public Pen()
        {
            SetNativePointer(NativeApi.Pen_Create_());
        }
        
        public Pen(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public void Initialize(PenDashStyle style, System.Drawing.Color color, float width)
        {
            CheckDisposed();
            NativeApi.Pen_Initialize_(NativePointer, style, color, width);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Pen_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Pen_Initialize_(IntPtr obj, PenDashStyle style, NativeApiTypes.Color color, float width);
            
        }
    }
}
