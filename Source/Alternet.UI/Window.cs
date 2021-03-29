using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;

namespace Alternet.UI
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class Window : Component
    {
        public Window()
        {
            NativePointer = NativeApi.Window_Create();
        }

        public IntPtr NativePointer { get; private set; }

        public bool IsDisposed { get; private set; }

        public void Show()
        {
            CheckDisposed();
            NativeApi.Window_Show(NativePointer);
        }

        public void AddControl(Control control)
        {
            CheckDisposed();
            NativeApi.Window_AddChildControl(NativePointer, control.NativePointer);
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
                    NativeApi.Window_Destroy(NativePointer);

                NativePointer = IntPtr.Zero;

                IsDisposed = true;
            }
        }

        public string Title
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
            public static extern IntPtr Window_Create();

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_Destroy(IntPtr obj);

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_Show(IntPtr obj);

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetTitle(IntPtr obj, string value);

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string Window_GetTitle(IntPtr obj);

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_AddChildControl(IntPtr obj, IntPtr control);
        }
    }
}