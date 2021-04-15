// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
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
        
        public string? Text
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
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.TextBoxEventCallbackType((obj, e) =>
                {
                    var w = NativeObject.GetFromNativePointer<TextBox>(obj, p => new TextBox(p));
                    if (w == null) return;
                    w.OnEvent(e);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.TextBox_SetEventCallback(sink);
            }
        }
        
        void OnEvent(NativeApi.TextBoxEvent e)
        {
            switch (e)
            {
                case NativeApi.TextBoxEvent.TextChanged: TextChanged?.Invoke(this, EventArgs.Empty); break;
                default: throw new Exception("Unexpected TextBoxEvent value: " + e);
            }
        }
        
        public event EventHandler? TextChanged;
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void TextBoxEventCallbackType(IntPtr obj, TextBoxEvent e);
            
            public enum TextBoxEvent
            {
                TextChanged,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetEventCallback(TextBoxEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr TextBox_Create();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string? TextBox_GetText(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetText(IntPtr obj, string? value);
            
        }
    }
}
