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
        private static readonly IdAndData<NativeObject> IdAndData = new();

        /*private static readonly Dictionary<IntPtr, NativeObject>
            InstancesByNativePointers = new();*/

        private IntPtr nativePointer;
        private bool isDisposed;

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

        public bool IsDisposed { get => isDisposed; private set => isDisposed = value; }

        public IntPtr NativePointer { get => nativePointer; private set => nativePointer = value; }

        public static T? GetFromNativePointer<T>(
            IntPtr pointer,
            Func<IntPtr, T>? fromPointerFactory)
            where T : NativeObject
        {
            if (pointer == IntPtr.Zero)
                return null;

            var id = GetIdNativeObjectPointer(pointer);
            if(id != 0)
            {
                var result = IdAndData.GetData(id);
                return (T?)result;
            }

            /*if (!InstancesByNativePointers.TryGetValue(pointer, out var w))
            {*/
            if (fromPointerFactory != null)
            {
                var newObject = fromPointerFactory(pointer);
                AddRefNativeObjectPointer(pointer);
                return newObject;
            }
            else
                return null;
            /*}*/

            /*return (T)w;*/
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void SetIdNativeObjectPointer(IntPtr ptr, int value)
        {
            if (ptr != IntPtr.Zero)
                NativeApi.Object_SetId(ptr, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static int GetIdNativeObjectPointer(IntPtr ptr)
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
            if (value == IntPtr.Zero && nativePointer != IntPtr.Zero)
            {
                var id = GetIdNativeObjectPointer(nativePointer);
                SetIdNativeObjectPointer(nativePointer, 0);
                IdAndData.FreeId(id);
                /*InstancesByNativePointers.Remove(NativePointer);*/
            }

            nativePointer = value;

            if (value != IntPtr.Zero)
            {
                var id = IdAndData.AllocID(this);
                SetIdNativeObjectPointer(value, id);
                /*InstancesByNativePointers.Add(NativePointer, this);*/
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                }

                if (nativePointer != IntPtr.Zero)
                {
                    SetNativePointer(IntPtr.Zero);
                    ReleaseNativeObjectPointer(nativePointer);
                }

                isDisposed = true;
            }
        }

        [Conditional("DEBUG")]
        protected void CheckDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(null);
        }

        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Object_SetId(IntPtr obj, int value);

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Object_GetId(IntPtr obj);

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Object_AddRef(IntPtr obj);

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Object_Release(IntPtr obj);
        }
    }
}