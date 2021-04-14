// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.Security;
namespace Alternet.UI.Native
{
    internal class Button : Control
    {
        public Button()
        {
            SetNativePointer(NativeApi.Button_Create());
            SetEventCallback();
        }
        
        public Button(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public string? Text
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
                var sink = new NativeApi.ButtonEventCallbackType((obj, e) =>
                {
                    var w = NativeObject.GetFromNativePointer<Button>(obj, p => new Button(p));
                    if (w == null) return;
                    w.OnEvent(e);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.Button_SetEventCallback(sink);
            }
        }
        
        void OnEvent(NativeApi.ButtonEvent e)
        {
            switch (e)
            {
                case NativeApi.ButtonEvent.Click: Click?.Invoke(this, EventArgs.Empty); break;
                default: throw new Exception("Unexpected ButtonEvent value: " + e);
            }
        }
        
        public event EventHandler? Click;
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void ButtonEventCallbackType(IntPtr obj, ButtonEvent e);
            
            public enum ButtonEvent
            {
                Click,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Button_SetEventCallback(ButtonEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Button_Create();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Button_Destroy(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string? Button_GetText(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Button_SetText(IntPtr obj, string? value);
            
        }
    }
}
