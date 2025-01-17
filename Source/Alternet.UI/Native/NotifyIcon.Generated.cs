// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class NotifyIcon : NativeObject
    {
        static NotifyIcon()
        {
            SetEventCallback();
        }
        
        public NotifyIcon()
        {
            SetNativePointer(NativeApi.NotifyIcon_Create_());
        }
        
        public NotifyIcon(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public string? Text
        {
            get
            {
                CheckDisposed();
                return NativeApi.NotifyIcon_GetText_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.NotifyIcon_SetText_(NativePointer, value);
            }
        }
        
        public Image? Icon
        {
            get
            {
                CheckDisposed();
                var _nnn = NativeApi.NotifyIcon_GetIcon_(NativePointer);
                var _mmm = NativeObject.GetFromNativePointer<Image>(_nnn, p => new Image(p));
                ReleaseNativeObjectPointer(_nnn);
                return _mmm;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.NotifyIcon_SetIcon_(NativePointer, value?.NativePointer ?? IntPtr.Zero);
            }
        }
        
        public Menu? Menu
        {
            get
            {
                CheckDisposed();
                var _nnn = NativeApi.NotifyIcon_GetMenu_(NativePointer);
                var _mmm = NativeObject.GetFromNativePointer<Menu>(_nnn, p => new Menu(p));
                ReleaseNativeObjectPointer(_nnn);
                return _mmm;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.NotifyIcon_SetMenu_(NativePointer, value?.NativePointer ?? IntPtr.Zero);
            }
        }
        
        public bool Visible
        {
            get
            {
                CheckDisposed();
                return NativeApi.NotifyIcon_GetVisible_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.NotifyIcon_SetVisible_(NativePointer, value);
            }
        }
        
        public static bool IsAvailable
        {
            get
            {
                return NativeApi.NotifyIcon_GetIsAvailable_();
            }
            
        }
        
        public bool IsIconInstalled
        {
            get
            {
                CheckDisposed();
                return NativeApi.NotifyIcon_GetIsIconInstalled_(NativePointer);
            }
            
        }
        
        public bool IsOk
        {
            get
            {
                CheckDisposed();
                return NativeApi.NotifyIcon_GetIsOk_(NativePointer);
            }
            
        }
        
        static GCHandle eventCallbackGCHandle;
        public static NotifyIcon? GlobalObject;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.NotifyIconEventCallbackType((obj, e, parameter) =>
                    UI.Application.HandleThreadExceptions(() =>
                    {
                        var w = NativeObject.GetFromNativePointer<NotifyIcon>(obj, p => new NotifyIcon(p));
                        w ??= GlobalObject;
                        if (w == null) return IntPtr.Zero;
                        return w.OnEvent(e, parameter);
                    }
                    )
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.NotifyIcon_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.NotifyIconEvent e, IntPtr parameter)
        {
            switch (e)
            {
                case NativeApi.NotifyIconEvent.Click:
                {
                    Click?.Invoke(); return IntPtr.Zero;
                }
                case NativeApi.NotifyIconEvent.DoubleClick:
                {
                    DoubleClick?.Invoke(); return IntPtr.Zero;
                }
                default: throw new Exception("Unexpected NotifyIconEvent value: " + e);
            }
        }
        
        public Action? Click;
        public Action? DoubleClick;
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr NotifyIconEventCallbackType(IntPtr obj, NotifyIconEvent e, IntPtr param);
            
            public enum NotifyIconEvent
            {
                Click,
                DoubleClick,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void NotifyIcon_SetEventCallback_(NotifyIconEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NotifyIcon_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string? NotifyIcon_GetText_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void NotifyIcon_SetText_(IntPtr obj, string? value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NotifyIcon_GetIcon_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void NotifyIcon_SetIcon_(IntPtr obj, IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NotifyIcon_GetMenu_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void NotifyIcon_SetMenu_(IntPtr obj, IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool NotifyIcon_GetVisible_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void NotifyIcon_SetVisible_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool NotifyIcon_GetIsAvailable_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool NotifyIcon_GetIsIconInstalled_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool NotifyIcon_GetIsOk_(IntPtr obj);
            
        }
    }
}
