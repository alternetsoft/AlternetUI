using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;

namespace Alternet.UI
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class Application : Component
    {
        private volatile bool isDisposed;

        public Application()
        {
            NativePointer = NativeApi.Application_Create();
        }

        public bool IsDisposed { get => isDisposed; private set => isDisposed = value; }

        internal IntPtr NativePointer { get; private set; }

        public void Run(Window window)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));
            CheckDisposed();

            NativeApi.Application_Run(NativePointer, window.NativePointer);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!IsDisposed)
            {
                if (NativePointer != IntPtr.Zero)
                {
                    NativeApi.Application_Destroy(NativePointer);
                    NativePointer = IntPtr.Zero;
                }

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

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Application_Create();

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Application_Destroy(IntPtr obj);

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Application_Run(IntPtr obj, IntPtr window);
        }
    }
}