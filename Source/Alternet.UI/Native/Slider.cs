// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class Slider : Control
    {
        public Slider()
        {
            SetNativePointer(NativeApi.Slider_Create());
        }
        
        public Slider(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public int Minimum
        {
            get
            {
                CheckDisposed();
                return NativeApi.Slider_GetMinimum(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Slider_SetMinimum(NativePointer, value);
            }
        }
        
        public int Maximum
        {
            get
            {
                CheckDisposed();
                return NativeApi.Slider_GetMaximum(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Slider_SetMaximum(NativePointer, value);
            }
        }
        
        public int Value
        {
            get
            {
                CheckDisposed();
                return NativeApi.Slider_GetValue(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Slider_SetValue(NativePointer, value);
            }
        }
        
        public int SmallChange
        {
            get
            {
                CheckDisposed();
                return NativeApi.Slider_GetSmallChange(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Slider_SetSmallChange(NativePointer, value);
            }
        }
        
        public int LargeChange
        {
            get
            {
                CheckDisposed();
                return NativeApi.Slider_GetLargeChange(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Slider_SetLargeChange(NativePointer, value);
            }
        }
        
        public int TickFrequency
        {
            get
            {
                CheckDisposed();
                return NativeApi.Slider_GetTickFrequency(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Slider_SetTickFrequency(NativePointer, value);
            }
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Slider_Create();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Slider_GetMinimum(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Slider_SetMinimum(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Slider_GetMaximum(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Slider_SetMaximum(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Slider_GetValue(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Slider_SetValue(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Slider_GetSmallChange(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Slider_SetSmallChange(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Slider_GetLargeChange(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Slider_SetLargeChange(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Slider_GetTickFrequency(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Slider_SetTickFrequency(IntPtr obj, int value);
            
        }
    }
}
