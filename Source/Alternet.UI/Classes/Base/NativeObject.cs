using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

namespace Alternet.UI.Native
{
    internal class NativeObject : DisposableObject
    {
        private static readonly Dictionary<IntPtr, NativeObject>
            InstancesByNativePointers = new();

        protected NativeObject()
        {
        }

        protected NativeObject(IntPtr nativePointer)
        {
            SetNativePointer(nativePointer);
        }

        public IntPtr NativePointer { get; private set; }

        public static Window? GetNativeWindow(Alternet.UI.Window? window)
        {
            if (window?.Handler is not WxWindowHandler handler)
                return null;
            var result = handler.NativeControl;
            return result;
        }

        public static T? GetFromNativePointer<T>(
            IntPtr pointer,
            Func<IntPtr, T>? fromPointerFactory)
            where T : NativeObject
        {
            if (pointer == IntPtr.Zero)
                return null;

            if (!InstancesByNativePointers.TryGetValue(pointer, out var w))
            {
                if (fromPointerFactory != null)
                {
                    var newObject = fromPointerFactory(pointer);
                    AddRefNativeObjectPointer(pointer);
                    return newObject;
                }
                else
                    return null;
            }

            return (T)w;
        }

        internal void SetNativePointerWeak(IntPtr nativePointer)
        {
            NativePointer = nativePointer;
            DisposeHandle = false;
        }

        protected static void AddRefNativeObjectPointer(IntPtr value)
        {
            if (value != IntPtr.Zero)
                NativeApi.Object_AddRef(value);
        }

        protected static void ReleaseNativeObjectPointer(IntPtr value)
        {
            if (value != IntPtr.Zero)
                NativeApi.Object_Release(value);
        }

        protected void SetNativePointer(IntPtr value)
        {
            // New value is the same as the old one.
            if (value == NativePointer)
                return;

            // New value is zero so just remove
            if(value == default)
            {
                InstancesByNativePointers.Remove(NativePointer);
                NativePointer = default;
                return;
            }

            // Old value is zero so just add
            if(NativePointer == default)
            {
                InstancesByNativePointers.Add(value, this);
                NativePointer = value;
                return;
            }

            InstancesByNativePointers[value] = this;
            NativePointer = value;
        }

        protected override void DisposeUnmanaged()
        {
            if (NativePointer != IntPtr.Zero && DisposeHandle)
            {
                ReleaseNativeObjectPointer(NativePointer);
                SetNativePointer(IntPtr.Zero);
            }
        }

        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Object_AddRef(IntPtr obj);

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Object_Release(IntPtr obj);
        }
    }
}