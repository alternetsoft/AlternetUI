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
            SetNativePointer(NativeApi.TextBox_Create());
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
                return NativeApi.TextBox_GetText(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetText(NativePointer, value);
            }
        }
        
        public bool EditControlOnly
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetEditControlOnly(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetEditControlOnly(NativePointer, value);
            }
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.TextBoxEventCallbackType((obj, e, param) =>
                {
                    var w = NativeObject.GetFromNativePointer<TextBox>(obj, p => new TextBox(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.TextBox_SetEventCallback(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.TextBoxEvent e)
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
            public static extern void TextBox_SetEventCallback(TextBoxEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr TextBox_Create();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string TextBox_GetText(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetText(IntPtr obj, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetEditControlOnly(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetEditControlOnly(IntPtr obj, bool value);
            
        }
    }
}
