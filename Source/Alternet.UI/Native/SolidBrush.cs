// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class SolidBrush : Brush
    {
        static SolidBrush()
        {
        }
        
        public SolidBrush()
        {
            SetNativePointer(NativeApi.SolidBrush_Create_());
        }
        
        public SolidBrush(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public void Initialize(Alternet.Drawing.Color color)
        {
            CheckDisposed();
            NativeApi.SolidBrush_Initialize_(NativePointer, color);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr SolidBrush_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void SolidBrush_Initialize_(IntPtr obj, NativeApiTypes.Color color);
            
        }
    }
}
