// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class ColorPicker : Control
    {
        static ColorPicker()
        {
            SetEventCallback();
        }
        
        public ColorPicker()
        {
            SetNativePointer(NativeApi.ColorPicker_Create_());
        }
        
        public ColorPicker(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public Alternet.Drawing.Color Value
        {
            get
            {
                CheckDisposed();
                return NativeApi.ColorPicker_GetValue_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ColorPicker_SetValue_(NativePointer, value);
            }
        }
        
        static GCHandle eventCallbackGCHandle;
        public static ColorPicker? GlobalObject;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.ColorPickerEventCallbackType((obj, e, parameter) =>
                    UI.Application.HandleThreadExceptions(() =>
                    {
                        var w = NativeObject.GetFromNativePointer<ColorPicker>(obj, p => new ColorPicker(p));
                        w ??= GlobalObject;
                        if (w == null) return IntPtr.Zero;
                        return w.OnEvent(e, parameter);
                    }
                    )
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.ColorPicker_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.ColorPickerEvent e, IntPtr parameter)
        {
            ValueChanged?.Invoke(); return IntPtr.Zero;
        }
        
        public Action? ValueChanged;
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr ColorPickerEventCallbackType(IntPtr obj, ColorPickerEvent e, IntPtr param);
            
            public enum ColorPickerEvent
            {
                ValueChanged,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ColorPicker_SetEventCallback_(ColorPickerEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ColorPicker_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.Color ColorPicker_GetValue_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ColorPicker_SetValue_(IntPtr obj, NativeApiTypes.Color value);
            
        }
    }
}
