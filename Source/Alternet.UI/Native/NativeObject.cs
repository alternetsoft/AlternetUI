using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace Alternet.UI.Native
{
    internal class NativeObject : IDisposable
    {
        protected NativeObject()
        {
        }

        protected NativeObject(IntPtr nativePointer)
        {
            SetNativePointer(nativePointer);
        }

        private static readonly Dictionary<IntPtr, NativeObject> instancesByNativePointers = new Dictionary<IntPtr, NativeObject>();

        ~NativeObject() => Dispose(disposing: false);

        public bool IsDisposed { get; private set; }

        public IntPtr NativePointer { get; private set; }

        public static T? GetFromNativePointer<T>(IntPtr pointer, Func<IntPtr, T>? fromPointerFactory) where T : NativeObject
        {
            if (pointer == IntPtr.Zero)
                return null;

            if (!instancesByNativePointers.TryGetValue(pointer, out var w))
            {
                if (fromPointerFactory != null)
                    return fromPointerFactory(pointer);
                else
                    return null;
            }

            return (T)w;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected void SetNativePointer(IntPtr value)
        {
            if (value == IntPtr.Zero)
                instancesByNativePointers.Remove(NativePointer);

            NativePointer = value;

            if (value != IntPtr.Zero)
                instancesByNativePointers.Add(NativePointer, this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                }

                if (NativePointer != IntPtr.Zero)
                {
                    NativeApi.Object_Release(NativePointer);
                    SetNativePointer(IntPtr.Zero);
                }

                IsDisposed = true;
            }
        }

        protected void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(null);
        }

        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Object_Release(IntPtr obj);
        }
    }
}