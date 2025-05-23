// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class DateTimePicker : Control
    {
        static DateTimePicker()
        {
            SetEventCallback();
        }
        
        public DateTimePicker()
        {
            SetNativePointer(NativeApi.DateTimePicker_Create_());
        }
        
        public DateTimePicker(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public bool HasBorder
        {
            get
            {
                CheckDisposed();
                return NativeApi.DateTimePicker_GetHasBorder_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.DateTimePicker_SetHasBorder_(NativePointer, value);
            }
        }
        
        public DateTime Value
        {
            get
            {
                CheckDisposed();
                return NativeApi.DateTimePicker_GetValue_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.DateTimePicker_SetValue_(NativePointer, value);
            }
        }
        
        public DateTime MinValue
        {
            get
            {
                CheckDisposed();
                return NativeApi.DateTimePicker_GetMinValue_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.DateTimePicker_SetMinValue_(NativePointer, value);
            }
        }
        
        public DateTime MaxValue
        {
            get
            {
                CheckDisposed();
                return NativeApi.DateTimePicker_GetMaxValue_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.DateTimePicker_SetMaxValue_(NativePointer, value);
            }
        }
        
        public int ValueKind
        {
            get
            {
                CheckDisposed();
                return NativeApi.DateTimePicker_GetValueKind_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.DateTimePicker_SetValueKind_(NativePointer, value);
            }
        }
        
        public int PopupKind
        {
            get
            {
                CheckDisposed();
                return NativeApi.DateTimePicker_GetPopupKind_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.DateTimePicker_SetPopupKind_(NativePointer, value);
            }
        }
        
        public void SetRange(bool useMinValue, bool useMaxValue)
        {
            CheckDisposed();
            NativeApi.DateTimePicker_SetRange_(NativePointer, useMinValue, useMaxValue);
        }
        
        static GCHandle eventCallbackGCHandle;
        public static DateTimePicker? GlobalObject;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.DateTimePickerEventCallbackType((obj, e, parameter) =>
                    UI.Application.HandleThreadExceptions(() =>
                    {
                        var w = NativeObject.GetFromNativePointer<DateTimePicker>(obj, p => new DateTimePicker(p));
                        w ??= GlobalObject;
                        if (w == null) return IntPtr.Zero;
                        return w.OnEvent(e, parameter);
                    }
                    )
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.DateTimePicker_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.DateTimePickerEvent e, IntPtr parameter)
        {
            OnPlatformEventValueChanged(); return IntPtr.Zero;
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr DateTimePickerEventCallbackType(IntPtr obj, DateTimePickerEvent e, IntPtr param);
            
            public enum DateTimePickerEvent
            {
                ValueChanged,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DateTimePicker_SetEventCallback_(DateTimePickerEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr DateTimePicker_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool DateTimePicker_GetHasBorder_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DateTimePicker_SetHasBorder_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.DateTime DateTimePicker_GetValue_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DateTimePicker_SetValue_(IntPtr obj, NativeApiTypes.DateTime value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.DateTime DateTimePicker_GetMinValue_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DateTimePicker_SetMinValue_(IntPtr obj, NativeApiTypes.DateTime value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.DateTime DateTimePicker_GetMaxValue_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DateTimePicker_SetMaxValue_(IntPtr obj, NativeApiTypes.DateTime value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int DateTimePicker_GetValueKind_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DateTimePicker_SetValueKind_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int DateTimePicker_GetPopupKind_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DateTimePicker_SetPopupKind_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DateTimePicker_SetRange_(IntPtr obj, bool useMinValue, bool useMaxValue);
            
        }
    }
}
