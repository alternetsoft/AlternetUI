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
                return NativeApi.CheckBox_GetText_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.CheckBox_SetText_(NativePointer, value);
            }
        }
        
        public bool IsChecked
        {
            get
            {
                CheckDisposed();
                return NativeApi.CheckBox_GetIsChecked_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.CheckBox_SetIsChecked_(NativePointer, value);
            }
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.CheckBoxEventCallbackType((obj, e, parameter) =>
                {
                    var w = NativeObject.GetFromNativePointer<CheckBox>(obj, p => new CheckBox(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e, parameter);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.CheckBox_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.CheckBoxEvent e, IntPtr parameter)
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
            public static extern void CheckBox_SetEventCallback_(CheckBoxEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CheckBox_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string CheckBox_GetText_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void CheckBox_SetText_(IntPtr obj, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool CheckBox_GetIsChecked_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void CheckBox_SetIsChecked_(IntPtr obj, bool value);
            
        }
    }
}
