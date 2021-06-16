// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class RadioButton : Control
    {
        public RadioButton()
        {
            SetNativePointer(NativeApi.RadioButton_Create());
            SetEventCallback();
        }
        
        public RadioButton(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public string? Text
        {
            get
            {
                CheckDisposed();
                return NativeApi.RadioButton_GetText(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.RadioButton_SetText(NativePointer, value);
            }
        }
        
        public bool IsChecked
        {
            get
            {
                CheckDisposed();
                return NativeApi.RadioButton_GetIsChecked(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.RadioButton_SetIsChecked(NativePointer, value);
            }
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.RadioButtonEventCallbackType((obj, e, param) =>
                {
                    var w = NativeObject.GetFromNativePointer<RadioButton>(obj, p => new RadioButton(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.RadioButton_SetEventCallback(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.RadioButtonEvent e)
        {
            switch (e)
            {
                case NativeApi.RadioButtonEvent.CheckedChanged:
                CheckedChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                default: throw new Exception("Unexpected RadioButtonEvent value: " + e);
            }
        }
        
        public event EventHandler? CheckedChanged;
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr RadioButtonEventCallbackType(IntPtr obj, RadioButtonEvent e, IntPtr param);
            
            public enum RadioButtonEvent
            {
                CheckedChanged,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void RadioButton_SetEventCallback(RadioButtonEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr RadioButton_Create();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string? RadioButton_GetText(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void RadioButton_SetText(IntPtr obj, string? value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool RadioButton_GetIsChecked(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void RadioButton_SetIsChecked(IntPtr obj, bool value);
            
        }
    }
}
