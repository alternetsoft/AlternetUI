// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.Security;
namespace Alternet.UI.Native
{
    internal class DrawingContext : NativeObject
    {
        private DrawingContext()
        {
        }
        
        public DrawingContext(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!IsDisposed)
            {
                if (NativePointer != IntPtr.Zero)
                {
                    NativeApi.DrawingContext_Destroy(NativePointer);
                    SetNativePointer(IntPtr.Zero);
                }
            }
        }
        
        public void FillRectangle(System.Drawing.RectangleF rectangle, System.Drawing.Color color)
        {
            CheckDisposed();
            NativeApi.DrawingContext_FillRectangle(NativePointer, rectangle, color);
        }
        
        public void DrawRectangle(System.Drawing.RectangleF rectangle, System.Drawing.Color color)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawRectangle(NativePointer, rectangle, color);
        }
        
        public void DrawText(string? text, System.Drawing.PointF origin, System.Drawing.Color color)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawText(NativePointer, text, origin, color);
        }
        
        public System.Drawing.SizeF MeasureText(string? text)
        {
            CheckDisposed();
            return NativeApi.DrawingContext_MeasureText(NativePointer, text);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_Destroy(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_FillRectangle(IntPtr obj, NativeApiTypes.RectangleF rectangle, NativeApiTypes.Color color);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawRectangle(IntPtr obj, NativeApiTypes.RectangleF rectangle, NativeApiTypes.Color color);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawText(IntPtr obj, string? text, NativeApiTypes.PointF origin, NativeApiTypes.Color color);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.SizeF DrawingContext_MeasureText(IntPtr obj, string? text);
            
        }
    }
}
