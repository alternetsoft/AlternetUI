/*

var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this); // For a Window

//Grab a handle for a specific window
Window w = new Window();
var handle = w.GetHandle();

//Grab a handle for the Window associated with the current process
var currentProcMainWindowHandle = GetCurrentProcMainWindowHandle();

*/

namespace Oceanside.Views.WinUI
{
    using Microsoft.UI.Xaml;
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using WinRT;

    public static class WindowHelpers
    {
        public static IntPtr GetCurrentProcMainWindowHandle()
        {
            using var process = Process.GetCurrentProcess();
            return process.MainWindowHandle;
        }

        public static IntPtr GetHandle(this Window window)
        {
            WindowWrapper windowWrapper = WindowWrapper.FromAbi(window.ThisPtr);
            return windowWrapper.WindowHandle;
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
        private interface IWindowNative
        {
            IntPtr WindowHandle { get; }
        }

        [Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
        private class WindowWrapper : IWindowNative
        {
            [Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
            public struct Vftbl
            {
                public delegate int _get_WindowHandle_0(IntPtr thisPtr, out IntPtr windowHandle);
                internal global::WinRT.Interop.IUnknownVftbl IUnknownVftbl;
                public _get_WindowHandle_0 get_WindowHandle_0;
                public static readonly Vftbl AbiToProjectionVftable;
                public static readonly IntPtr AbiToProjectionVftablePtr;

                static Vftbl()
                {
                    AbiToProjectionVftable = new Vftbl
                    {
                        IUnknownVftbl = global::WinRT.Interop.IUnknownVftbl.AbiToProjectionVftbl,
                        get_WindowHandle_0 = Do_Abi_get_WindowHandle_0
                    };
                    AbiToProjectionVftablePtr = Marshal.AllocHGlobal(Marshal.SizeOf<Vftbl>());
                    Marshal.StructureToPtr(AbiToProjectionVftable, AbiToProjectionVftablePtr, false);
                }

                private static int Do_Abi_get_WindowHandle_0(IntPtr thisPtr, out IntPtr windowHandle)
                {
                    windowHandle = default;
                    try
                    {
                        windowHandle = ComWrappersSupport.FindObject<IWindowNative>(thisPtr).WindowHandle;
                    }
                    catch (Exception ex)
                    {
                        return Marshal.GetHRForException(ex);
                    }
                    return 0;
                }
            }
            internal static ObjectReference<Vftbl> FromAbi(IntPtr thisPtr)
            {
                return ObjectReference<Vftbl>.FromAbi(thisPtr);
            }

            public static implicit operator WindowWrapper(IObjectReference obj)
            {
                return (obj != null) ? new WindowWrapper(obj) : null;
            }

            protected readonly ObjectReference<Vftbl> _obj;
            public IObjectReference ObjRef => _obj;
            public IntPtr ThisPtr => _obj.ThisPtr;
            public ObjectReference<I> AsInterface<I>()
            {
                return _obj.As<I>();
            }

            public A As<A>()
            {
                return _obj.AsType<A>();
            }

            public WindowWrapper(IObjectReference obj) : this(obj.As<Vftbl>()) { }
            internal WindowWrapper(ObjectReference<Vftbl> obj)
            {
                _obj = obj;
            }

            public IntPtr WindowHandle
            {
                get
                {
                    Marshal.ThrowExceptionForHR(_obj.Vftbl.get_WindowHandle_0(ThisPtr, out var windowHandle));

                    return windowHandle;
                }
            }
        }
    }
}