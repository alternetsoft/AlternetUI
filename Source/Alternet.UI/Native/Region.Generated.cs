// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class Region : NativeObject
    {
        static Region()
        {
        }
        
        public Region()
        {
            SetNativePointer(NativeApi.Region_Create_());
        }
        
        public Region(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public void Clear()
        {
            CheckDisposed();
            NativeApi.Region_Clear_(NativePointer);
        }
        
        public int ContainsPoint(Alternet.Drawing.PointD pt)
        {
            CheckDisposed();
            return NativeApi.Region_ContainsPoint_(NativePointer, pt);
        }
        
        public int ContainsRect(Alternet.Drawing.RectD rect)
        {
            CheckDisposed();
            return NativeApi.Region_ContainsRect_(NativePointer, rect);
        }
        
        public bool IsEmpty()
        {
            CheckDisposed();
            return NativeApi.Region_IsEmpty_(NativePointer);
        }
        
        public bool IsOk()
        {
            CheckDisposed();
            return NativeApi.Region_IsOk_(NativePointer);
        }
        
        public void InitializeWithRegion(Region region)
        {
            CheckDisposed();
            NativeApi.Region_InitializeWithRegion_(NativePointer, region.NativePointer);
        }
        
        public void InitializeWithRect(Alternet.Drawing.RectD rect)
        {
            CheckDisposed();
            NativeApi.Region_InitializeWithRect_(NativePointer, rect);
        }
        
        public void InitializeWithPolygon(Alternet.Drawing.PointD[] points, FillMode fillMode)
        {
            CheckDisposed();
            NativeApi.Region_InitializeWithPolygon_(NativePointer, points, points.Length, fillMode);
        }
        
        public void IntersectWithRect(Alternet.Drawing.RectD rect)
        {
            CheckDisposed();
            NativeApi.Region_IntersectWithRect_(NativePointer, rect);
        }
        
        public void IntersectWithRegion(Region region)
        {
            CheckDisposed();
            NativeApi.Region_IntersectWithRegion_(NativePointer, region.NativePointer);
        }
        
        public void UnionWithRect(Alternet.Drawing.RectD rect)
        {
            CheckDisposed();
            NativeApi.Region_UnionWithRect_(NativePointer, rect);
        }
        
        public void UnionWithRegion(Region region)
        {
            CheckDisposed();
            NativeApi.Region_UnionWithRegion_(NativePointer, region.NativePointer);
        }
        
        public void XorWithRect(Alternet.Drawing.RectD rect)
        {
            CheckDisposed();
            NativeApi.Region_XorWithRect_(NativePointer, rect);
        }
        
        public void XorWithRegion(Region region)
        {
            CheckDisposed();
            NativeApi.Region_XorWithRegion_(NativePointer, region.NativePointer);
        }
        
        public void SubtractRect(Alternet.Drawing.RectD rect)
        {
            CheckDisposed();
            NativeApi.Region_SubtractRect_(NativePointer, rect);
        }
        
        public void SubtractRegion(Region region)
        {
            CheckDisposed();
            NativeApi.Region_SubtractRegion_(NativePointer, region.NativePointer);
        }
        
        public void Translate(double dx, double dy)
        {
            CheckDisposed();
            NativeApi.Region_Translate_(NativePointer, dx, dy);
        }
        
        public Alternet.Drawing.RectD GetBounds()
        {
            CheckDisposed();
            return NativeApi.Region_GetBounds_(NativePointer);
        }
        
        public bool IsEqualTo(Region other)
        {
            CheckDisposed();
            return NativeApi.Region_IsEqualTo_(NativePointer, other.NativePointer);
        }
        
        public int GetHashCode_()
        {
            CheckDisposed();
            return NativeApi.Region_GetHashCode__(NativePointer);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Region_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Region_Clear_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Region_ContainsPoint_(IntPtr obj, Alternet.Drawing.PointD pt);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Region_ContainsRect_(IntPtr obj, Alternet.Drawing.RectD rect);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Region_IsEmpty_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Region_IsOk_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Region_InitializeWithRegion_(IntPtr obj, IntPtr region);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Region_InitializeWithRect_(IntPtr obj, Alternet.Drawing.RectD rect);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Region_InitializeWithPolygon_(IntPtr obj, Alternet.Drawing.PointD[] points, int pointsCount, FillMode fillMode);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Region_IntersectWithRect_(IntPtr obj, Alternet.Drawing.RectD rect);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Region_IntersectWithRegion_(IntPtr obj, IntPtr region);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Region_UnionWithRect_(IntPtr obj, Alternet.Drawing.RectD rect);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Region_UnionWithRegion_(IntPtr obj, IntPtr region);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Region_XorWithRect_(IntPtr obj, Alternet.Drawing.RectD rect);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Region_XorWithRegion_(IntPtr obj, IntPtr region);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Region_SubtractRect_(IntPtr obj, Alternet.Drawing.RectD rect);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Region_SubtractRegion_(IntPtr obj, IntPtr region);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Region_Translate_(IntPtr obj, double dx, double dy);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.RectD Region_GetBounds_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Region_IsEqualTo_(IntPtr obj, IntPtr other);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Region_GetHashCode__(IntPtr obj);
            
        }
    }
}