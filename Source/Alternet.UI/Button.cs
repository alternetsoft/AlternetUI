using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace Alternet.UI
{
    public class Button : Control
    {
        static GCHandle eventCallbackGCHandle;

        public Button()
        {
            NativePointer = NativeApi.Button_Create();
            instancesByNativePointers[NativePointer] = this;

            SetEventCallback();
        }

        static readonly Dictionary<IntPtr, Button> instancesByNativePointers = new Dictionary<IntPtr, Button>();

        static Button? TryGetFromNativePointer(IntPtr pointer)
        {
            if (!instancesByNativePointers.TryGetValue(pointer, out var w))
                return null;
            return w;
        }

        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.ButtonEventCallbackType((obj, e, param) =>
                {
                    var w = TryGetFromNativePointer(obj);
                    if (w == null)
                    {
                        return 0;
                    }

                    return w.OnEvent(e, param);
                });

                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.Button_SetEventCallback(sink);
            }
        }

        int OnEvent(NativeApi.ButtonEvent e, IntPtr param)
        {
            switch (e)
            {
                case NativeApi.ButtonEvent.Click:
                    OnClick(EventArgs.Empty);
                    break;

                default:
                    throw new Exception("Unexpected NativeApi.ButtonEvent value: " + e);
            }

            return 0;
        }

        protected virtual void OnClick(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            Click?.Invoke(this, e);
        }

        public event EventHandler? Click;

        public bool IsDisposed { get; private set; }

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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!IsDisposed)
            {
                if (disposing)
                {
                }

                if (NativePointer != IntPtr.Zero)
                    NativeApi.Button_Destroy(NativePointer);

                NativePointer = IntPtr.Zero;

                IsDisposed = true;
            }
        }

        private void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(null);
        }

        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi()
            {
                Initialize();
            }

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate int ButtonEventCallbackType(IntPtr obj, ButtonEvent e, IntPtr param);

            public enum ButtonEvent
            {
                Click
            }

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Button_Create();

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Button_Destroy(IntPtr obj);

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Button_SetText(IntPtr obj, string value);

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string Button_GetText(IntPtr obj);

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Button_SetEventCallback(ButtonEventCallbackType callback);
        }
    }
}