// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class LinearGradientBrush : Brush
    {
        static LinearGradientBrush()
        {
        }
        
        public LinearGradientBrush()
        {
            SetNativePointer(NativeApi.LinearGradientBrush_Create_());
        }
        
        public LinearGradientBrush(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public void Initialize(Alternet.Drawing.PointD startPoint, Alternet.Drawing.PointD endPoint, Alternet.Drawing.Color[] gradientStopsColors, System.Double[] gradientStopsOffsets)
        {
            CheckDisposed();
            NativeApi.LinearGradientBrush_Initialize_(NativePointer, startPoint, endPoint, Array.ConvertAll<Alternet.Drawing.Color, NativeApiTypes.Color>(gradientStopsColors, x => x), gradientStopsColors.Length, gradientStopsOffsets, gradientStopsOffsets.Length);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr LinearGradientBrush_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void LinearGradientBrush_Initialize_(IntPtr obj, Alternet.Drawing.PointD startPoint, Alternet.Drawing.PointD endPoint, NativeApiTypes.Color[] gradientStopsColors, int gradientStopsColorsCount, System.Double[] gradientStopsOffsets, int gradientStopsOffsetsCount);
            
        }
    }
}
