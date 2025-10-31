using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

namespace Alternet.UI.Native
{
    internal class NativeObject : BaseObject, IDisposable
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

        ~NativeObject()
        {
            Dispose(disposing: false);
        }

        public bool IsDisposed { get; private set; }

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

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
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

            /*Debug.WriteLineIf(true, $"SetNativePointer: {this.GetType()} : {value}");*/
        }

        /// <summary>
        /// Override to dispose managed resources.
        /// Here we dispose all used object references.
        /// </summary>
        protected virtual void DisposeManaged()
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    DisposeManaged();
                }

                if (NativePointer != IntPtr.Zero)
                {
                    ReleaseNativeObjectPointer(NativePointer);
                    SetNativePointer(IntPtr.Zero);
                }

                IsDisposed = true;
            }
        }

        [Conditional("DEBUG")]
        protected void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(null);
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