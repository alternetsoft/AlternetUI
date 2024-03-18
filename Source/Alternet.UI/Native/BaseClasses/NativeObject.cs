using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
#if NETFRAMEWORK  // Issue #27
            if (Alternet.UI.Application.Terminating)
                return;
#endif
            Dispose(disposing: false);
        }

        public bool NoDispose { get; set; }

        public bool IsDisposed { get; private set; }

        public IntPtr NativePointer { get; private set; }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void SetIdNativeObjectPointer(IntPtr ptr, ulong value)
        {
            if (ptr != IntPtr.Zero)
                NativeApi.Object_SetId(ptr, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static ulong GetIdNativeObjectPointer(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return 0;
            else
                return NativeApi.Object_GetId(ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void AddRefNativeObjectPointer(IntPtr value)
        {
            if (value != IntPtr.Zero)
                NativeApi.Object_AddRef(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void ReleaseNativeObjectPointer(IntPtr value)
        {
            if (value != IntPtr.Zero)
                NativeApi.Object_Release(value);
        }

        protected void SetNativePointer(IntPtr value)
        {
            if (value == IntPtr.Zero)
            {
                InstancesByNativePointers.Remove(NativePointer);
            }

            NativePointer = value;

            if (value != IntPtr.Zero)
            {
                InstancesByNativePointers.Add(NativePointer, this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                }

                if (NativePointer != IntPtr.Zero && !NoDispose)
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
            public static extern void Object_SetId(IntPtr obj, ulong value);

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern ulong Object_GetId(IntPtr obj);

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Object_AddRef(IntPtr obj);

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Object_Release(IntPtr obj);
        }
    }
}