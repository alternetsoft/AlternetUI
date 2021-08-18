// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
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
        
        public void FillRectangle(System.Drawing.RectangleF rectangle, Brush brush)
        {
            CheckDisposed();
            NativeApi.DrawingContext_FillRectangle_(NativePointer, rectangle, brush.NativePointer);
        }
        
        public void DrawRectangle(System.Drawing.RectangleF rectangle, Pen pen)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawRectangle_(NativePointer, rectangle, pen.NativePointer);
        }
        
        public void DrawText(string text, System.Drawing.PointF origin, Font font, Brush brush)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawText_(NativePointer, text, origin, font.NativePointer, brush.NativePointer);
        }
        
        public void DrawImage(Image image, System.Drawing.PointF origin)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawImage_(NativePointer, image.NativePointer, origin);
        }
        
        public System.Drawing.SizeF MeasureText(string text, Font font)
        {
            CheckDisposed();
            return NativeApi.DrawingContext_MeasureText_(NativePointer, text, font.NativePointer);
        }
        
        public void PushTransform(System.Drawing.SizeF translation)
        {
            CheckDisposed();
            NativeApi.DrawingContext_PushTransform_(NativePointer, translation);
        }
        
        public void Pop()
        {
            CheckDisposed();
            NativeApi.DrawingContext_Pop_(NativePointer);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_FillRectangle_(IntPtr obj, NativeApiTypes.RectangleF rectangle, IntPtr brush);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawRectangle_(IntPtr obj, NativeApiTypes.RectangleF rectangle, IntPtr pen);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawText_(IntPtr obj, string text, NativeApiTypes.PointF origin, IntPtr font, IntPtr brush);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawImage_(IntPtr obj, IntPtr image, NativeApiTypes.PointF origin);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.SizeF DrawingContext_MeasureText_(IntPtr obj, string text, IntPtr font);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_PushTransform_(IntPtr obj, NativeApiTypes.SizeF translation);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_Pop_(IntPtr obj);
            
        }
    }
}
