// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2023 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class Timer : NativeObject
    {
        static Timer()
        {
            SetEventCallback();
        }
        
        public Timer()
        {
            SetNativePointer(NativeApi.Timer_Create_());
        }
        
        public Timer(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public bool Enabled
        {
            get
            {
                CheckDisposed();
                return NativeApi.Timer_GetEnabled_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Timer_SetEnabled_(NativePointer, value);
            }
        }
        
        public int Interval
        {
            get
            {
                CheckDisposed();
                return NativeApi.Timer_GetInterval_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Timer_SetInterval_(NativePointer, value);
            }
        }
        
        public bool AutoReset
        {
            get
            {
                CheckDisposed();
                return NativeApi.Timer_GetAutoReset_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Timer_SetAutoReset_(NativePointer, value);
            }
        }
        
        public void Restart()
        {
            CheckDisposed();
            NativeApi.Timer_Restart_(NativePointer);
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.TimerEventCallbackType((obj, e, parameter) =>
                {
                    var w = NativeObject.GetFromNativePointer<Timer>(obj, p => new Timer(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e, parameter);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.Timer_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.TimerEvent e, IntPtr parameter)
        {
            switch (e)
            {
                case NativeApi.TimerEvent.Tick:
                {
                    Tick?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                }
                default: throw new Exception("Unexpected TimerEvent value: " + e);
            }
        }
        
        public event EventHandler? Tick;
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr TimerEventCallbackType(IntPtr obj, TimerEvent e, IntPtr param);
            
            public enum TimerEvent
            {
                Tick,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Timer_SetEventCallback_(TimerEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Timer_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Timer_GetEnabled_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Timer_SetEnabled_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Timer_GetInterval_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Timer_SetInterval_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Timer_GetAutoReset_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Timer_SetAutoReset_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Timer_Restart_(IntPtr obj);
            
        }
    }
}
