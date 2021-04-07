// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.Security;
namespace Alternet.UI.Native
{
    internal class DrawingContext : NativeObject
    {
        public DrawingContext()
        {
            SetNativePointer(NativeApi.DrawingContext_Create());
        }
        
        public DrawingContext(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public void FillRectangle(System.Drawing.RectangleF rectangle, System.Drawing.Color color)
        {
            CheckDisposed();
            NativeApi.DrawingContext_FillRectangle(NativePointer, rectangle, color);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr DrawingContext_Create();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_Destroy(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_FillRectangle(IntPtr obj, NativeApiTypes.RectangleF rectangle, NativeApiTypes.Color color);
            
        }
    }
}
