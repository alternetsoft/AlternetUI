// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class CheckBox : Control
    {
        public CheckBox()
        {
            SetNativePointer(NativeApi.CheckBox_Create_());
            SetEventCallback();
        }
        
        public CheckBox(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public string Text
        {
            get
            {
                CheckDisposed();
                return NativeApi.CheckBox_GetText(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.CheckBox_SetText(NativePointer, value);
            }
        }
        
        public bool IsChecked
        {
            get
            {
                CheckDisposed();
                return NativeApi.CheckBox_GetIsChecked(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.CheckBox_SetIsChecked(NativePointer, value);
            }
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.CheckBoxEventCallbackType((obj, e, param) =>
                {
                    var w = NativeObject.GetFromNativePointer<CheckBox>(obj, p => new CheckBox(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.CheckBox_SetEventCallback(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.CheckBoxEvent e)
        {
            switch (e)
            {
                case NativeApi.CheckBoxEvent.CheckedChanged:
                CheckedChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                default: throw new Exception("Unexpected CheckBoxEvent value: " + e);
            }
        }
        
        public event EventHandler? CheckedChanged;
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr CheckBoxEventCallbackType(IntPtr obj, CheckBoxEvent e, IntPtr param);
            
            public enum CheckBoxEvent
            {
                CheckedChanged,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void CheckBox_SetEventCallback(CheckBoxEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CheckBox_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string CheckBox_GetText(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void CheckBox_SetText(IntPtr obj, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool CheckBox_GetIsChecked(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void CheckBox_SetIsChecked(IntPtr obj, bool value);
            
        }
    }
}
