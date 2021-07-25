// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class TextBox : Control
    {
        public TextBox()
        {
            SetNativePointer(NativeApi.TextBox_Create_());
            SetEventCallback();
        }
        
        public TextBox(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public string Text
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetText_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetText_(NativePointer, value);
            }
        }
        
        public bool EditControlOnly
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetEditControlOnly_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetEditControlOnly_(NativePointer, value);
            }
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.TextBoxEventCallbackType((obj, e, parameter) =>
                {
                    var w = NativeObject.GetFromNativePointer<TextBox>(obj, p => new TextBox(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e, parameter);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.TextBox_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.TextBoxEvent e, IntPtr parameter)
        {
            switch (e)
            {
                case NativeApi.TextBoxEvent.TextChanged:
                TextChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                default: throw new Exception("Unexpected TextBoxEvent value: " + e);
            }
        }
        
        public event EventHandler? TextChanged;
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr TextBoxEventCallbackType(IntPtr obj, TextBoxEvent e, IntPtr param);
            
            public enum TextBoxEvent
            {
                TextChanged,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetEventCallback_(TextBoxEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr TextBox_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string TextBox_GetText_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetText_(IntPtr obj, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetEditControlOnly_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetEditControlOnly_(IntPtr obj, bool value);
            
        }
    }
}
