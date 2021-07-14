// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections.Generic;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class InputStream : ManagedServerObject
    {
        private static long GetLength_Trampoline(IntPtr obj)
        {
            return ((InputStream)GCHandle.FromIntPtr(obj).Target).Length;
        }
        
        private static bool GetIsOK_Trampoline(IntPtr obj)
        {
            return ((InputStream)GCHandle.FromIntPtr(obj).Target).IsOK;
        }
        
        private static bool GetIsSeekable_Trampoline(IntPtr obj)
        {
            return ((InputStream)GCHandle.FromIntPtr(obj).Target).IsSeekable;
        }
        
        private static long GetPosition_Trampoline(IntPtr obj)
        {
            return ((InputStream)GCHandle.FromIntPtr(obj).Target).Position;
        }
        private static void SetPosition_Trampoline(IntPtr obj, long value)
        {
            ((InputStream)GCHandle.FromIntPtr(obj).Target).Position = value;
        }
        
        
        private static System.IntPtr Read_Trampoline(IntPtr obj, System.Byte[] buffer, System.IntPtr length)
        {
            return ((InputStream)GCHandle.FromIntPtr(obj).Target).Read(buffer, length);
        }
        
        static GCHandle trampolineLocatorCallbackGCHandle;
        static readonly Dictionary<NativeApi.InputStreamTrampoline, (GCHandle GCHandle, IntPtr Pointer)> trampolineHandles = new Dictionary<NativeApi.InputStreamTrampoline, (GCHandle GCHandle, IntPtr Pointer)>();
        
        static InputStream() { SetTrampolineLocatorCallback(); }
        static void SetTrampolineLocatorCallback()
        {
            if (!trampolineLocatorCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.InputStreamTrampolineLocatorCallbackType(trampoline =>
                {
                    switch (trampoline)
                    {
                        case NativeApi.InputStreamTrampoline.Read:
                        {
                            if (!trampolineHandles.TryGetValue(NativeApi.InputStreamTrampoline.Read, out var handle))
                            {
                                var @delegate = (NativeApi.TRead)Read_Trampoline;
                                handle = (GCHandle.Alloc(@delegate), Marshal.GetFunctionPointerForDelegate(@delegate));
                                trampolineHandles.Add(trampoline, handle);
                            }
                            return handle.Pointer;
                        }
                        case NativeApi.InputStreamTrampoline.GetLength:
                        {
                            if (!trampolineHandles.TryGetValue(NativeApi.InputStreamTrampoline.GetLength, out var handle))
                            {
                                var @delegate = (NativeApi.TGetLength)GetLength_Trampoline;
                                handle = (GCHandle.Alloc(@delegate), Marshal.GetFunctionPointerForDelegate(@delegate));
                                trampolineHandles.Add(trampoline, handle);
                            }
                            return handle.Pointer;
                        }
                        case NativeApi.InputStreamTrampoline.GetIsOK:
                        {
                            if (!trampolineHandles.TryGetValue(NativeApi.InputStreamTrampoline.GetIsOK, out var handle))
                            {
                                var @delegate = (NativeApi.TGetIsOK)GetIsOK_Trampoline;
                                handle = (GCHandle.Alloc(@delegate), Marshal.GetFunctionPointerForDelegate(@delegate));
                                trampolineHandles.Add(trampoline, handle);
                            }
                            return handle.Pointer;
                        }
                        case NativeApi.InputStreamTrampoline.GetIsSeekable:
                        {
                            if (!trampolineHandles.TryGetValue(NativeApi.InputStreamTrampoline.GetIsSeekable, out var handle))
                            {
                                var @delegate = (NativeApi.TGetIsSeekable)GetIsSeekable_Trampoline;
                                handle = (GCHandle.Alloc(@delegate), Marshal.GetFunctionPointerForDelegate(@delegate));
                                trampolineHandles.Add(trampoline, handle);
                            }
                            return handle.Pointer;
                        }
                        case NativeApi.InputStreamTrampoline.GetPosition:
                        {
                            if (!trampolineHandles.TryGetValue(NativeApi.InputStreamTrampoline.GetPosition, out var handle))
                            {
                                var @delegate = (NativeApi.TGetPosition)GetPosition_Trampoline;
                                handle = (GCHandle.Alloc(@delegate), Marshal.GetFunctionPointerForDelegate(@delegate));
                                trampolineHandles.Add(trampoline, handle);
                            }
                            return handle.Pointer;
                        }
                        case NativeApi.InputStreamTrampoline.SetPosition:
                        {
                            if (!trampolineHandles.TryGetValue(NativeApi.InputStreamTrampoline.SetPosition, out var handle))
                            {
                                var @delegate = (NativeApi.TSetPosition)SetPosition_Trampoline;
                                handle = (GCHandle.Alloc(@delegate), Marshal.GetFunctionPointerForDelegate(@delegate));
                                trampolineHandles.Add(trampoline, handle);
                            }
                            return handle.Pointer;
                        }
                        default: throw new Exception("Unexpected InputStreamTrampoline value: " + trampoline);
                    }
                }
                );
                trampolineLocatorCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.InputStream_SetTrampolineLocatorCallback(sink);
            }
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr InputStreamTrampolineLocatorCallbackType(InputStreamTrampoline value);
            
            public enum InputStreamTrampoline
            {
                Read,
                GetLength,
                GetIsOK,
                GetIsSeekable,
                GetPosition,
                SetPosition,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void InputStream_SetTrampolineLocatorCallback(InputStreamTrampolineLocatorCallbackType callback);
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public delegate long TGetLength(IntPtr obj);
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public delegate bool TGetIsOK(IntPtr obj);
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public delegate bool TGetIsSeekable(IntPtr obj);
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public delegate long TGetPosition(IntPtr obj);
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public delegate void TSetPosition(IntPtr obj, long value);
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public delegate System.IntPtr TRead(IntPtr obj, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]System.Byte[] buffer, System.IntPtr length);
            
        }
    }
}
