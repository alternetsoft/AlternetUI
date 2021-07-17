// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class Button : Control
    {
        public Button()
        {
            SetNativePointer(NativeApi.Button_Create_());
            SetEventCallback();
        }
        
        public Button(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public string Text
        {
            get
            {
                CheckDisposed();
                return NativeApi.Button_GetText(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Button_SetText(NativePointer, value);
            }
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.ButtonEventCallbackType((obj, e, param) =>
                {
                    var w = NativeObject.GetFromNativePointer<Button>(obj, p => new Button(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.Button_SetEventCallback(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.ButtonEvent e)
        {
            switch (e)
            {
                case NativeApi.ButtonEvent.Click:
                Click?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                default: throw new Exception("Unexpected ButtonEvent value: " + e);
            }
        }
        
        public event EventHandler? Click;
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr ButtonEventCallbackType(IntPtr obj, ButtonEvent e, IntPtr param);
            
            public enum ButtonEvent
            {
                Click,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Button_SetEventCallback(ButtonEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Button_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string Button_GetText(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Button_SetText(IntPtr obj, string value);
            
        }
    }
}
