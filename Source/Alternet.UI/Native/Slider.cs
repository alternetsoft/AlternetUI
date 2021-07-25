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
            SetNativePointer(NativeApi.Slider_Create_());
            SetEventCallback();
        }
        
        public Slider(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public int Minimum
        {
            get
            {
                CheckDisposed();
                return NativeApi.Slider_GetMinimum_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Slider_SetMinimum_(NativePointer, value);
            }
        }
        
        public int Maximum
        {
            get
            {
                CheckDisposed();
                return NativeApi.Slider_GetMaximum_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Slider_SetMaximum_(NativePointer, value);
            }
        }
        
        public int Value
        {
            get
            {
                CheckDisposed();
                return NativeApi.Slider_GetValue_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Slider_SetValue_(NativePointer, value);
            }
        }
        
        public int SmallChange
        {
            get
            {
                CheckDisposed();
                return NativeApi.Slider_GetSmallChange_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Slider_SetSmallChange_(NativePointer, value);
            }
        }
        
        public int LargeChange
        {
            get
            {
                CheckDisposed();
                return NativeApi.Slider_GetLargeChange_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Slider_SetLargeChange_(NativePointer, value);
            }
        }
        
        public int TickFrequency
        {
            get
            {
                CheckDisposed();
                return NativeApi.Slider_GetTickFrequency_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Slider_SetTickFrequency_(NativePointer, value);
            }
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.SliderEventCallbackType((obj, e, parameter) =>
                {
                    var w = NativeObject.GetFromNativePointer<Slider>(obj, p => new Slider(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e, parameter);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.Slider_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.SliderEvent e, IntPtr parameter)
        {
            switch (e)
            {
                case NativeApi.SliderEvent.ValueChanged:
                ValueChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                default: throw new Exception("Unexpected SliderEvent value: " + e);
            }
        }
        
        public event EventHandler? ValueChanged;
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr SliderEventCallbackType(IntPtr obj, SliderEvent e, IntPtr param);
            
            public enum SliderEvent
            {
                ValueChanged,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Slider_SetEventCallback_(SliderEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Slider_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Slider_GetMinimum_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Slider_SetMinimum_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Slider_GetMaximum_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Slider_SetMaximum_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Slider_GetValue_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Slider_SetValue_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Slider_GetSmallChange_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Slider_SetSmallChange_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Slider_GetLargeChange_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Slider_SetLargeChange_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Slider_GetTickFrequency_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Slider_SetTickFrequency_(IntPtr obj, int value);
            
        }
    }
}
