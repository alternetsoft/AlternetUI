// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class Window : Control
    {
        public Window()
        {
            SetNativePointer(NativeApi.Window_Create());
            SetEventCallback();
        }
        
        public Window(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public string? Title
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetTitle(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetTitle(NativePointer, value);
            }
        }
        
        public WindowStartPosition WindowStartPosition
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetWindowStartPosition(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetWindowStartPosition(NativePointer, value);
            }
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.WindowEventCallbackType((obj, e, param) =>
                {
                    var w = NativeObject.GetFromNativePointer<Window>(obj, p => new Window(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.Window_SetEventCallback(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.WindowEvent e)
        {
            switch (e)
            {
                case NativeApi.WindowEvent.Closing:
                {
                    var cea = new CancelEventArgs();
                    Closing?.Invoke(this, cea);
                    return cea.Cancel ? new IntPtr(1) : IntPtr.Zero;
                }
                default: throw new Exception("Unexpected WindowEvent value: " + e);
            }
        }
        
        public event EventHandler<CancelEventArgs>? Closing;
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr WindowEventCallbackType(IntPtr obj, WindowEvent e, IntPtr param);
            
            public enum WindowEvent
            {
                Closing,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetEventCallback(WindowEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Window_Create();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string? Window_GetTitle(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetTitle(IntPtr obj, string? value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern WindowStartPosition Window_GetWindowStartPosition(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetWindowStartPosition(IntPtr obj, WindowStartPosition value);
            
        }
    }
}
