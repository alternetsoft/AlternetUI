using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;

namespace Alternet.UI.Native
{
    internal class Window : NativeObject
    {
        public Window()
        {
            NativePointer = NativeApi.Window_Create();
        }

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