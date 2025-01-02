// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class ScrollBar : Control
    {
        static ScrollBar()
        {
            SetEventCallback();
        }
        
        public ScrollBar()
        {
            SetNativePointer(NativeApi.ScrollBar_Create_());
        }
        
        public ScrollBar(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public int EventTypeID
        {
            get
            {
                CheckDisposed();
                return NativeApi.ScrollBar_GetEventTypeID_(NativePointer);
            }
            
        }
        
        public int EventOldPos
        {
            get
            {
                CheckDisposed();
                return NativeApi.ScrollBar_GetEventOldPos_(NativePointer);
            }
            
        }
        
        public int EventNewPos
        {
            get
            {
                CheckDisposed();
                return NativeApi.ScrollBar_GetEventNewPos_(NativePointer);
            }
            
        }
        
        public bool IsVertical
        {
            get
            {
                CheckDisposed();
                return NativeApi.ScrollBar_GetIsVertical_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ScrollBar_SetIsVertical_(NativePointer, value);
            }
        }
        
        public int ThumbPosition
        {
            get
            {
                CheckDisposed();
                return NativeApi.ScrollBar_GetThumbPosition_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ScrollBar_SetThumbPosition_(NativePointer, value);
            }
        }
        
        public int Range
        {
            get
            {
                CheckDisposed();
                return NativeApi.ScrollBar_GetRange_(NativePointer);
            }
            
        }
        
        public int ThumbSize
        {
            get
            {
                CheckDisposed();
                return NativeApi.ScrollBar_GetThumbSize_(NativePointer);
            }
            
        }
        
        public int PageSize
        {
            get
            {
                CheckDisposed();
                return NativeApi.ScrollBar_GetPageSize_(NativePointer);
            }
            
        }
        
        public void SetScrollbar(int position, int thumbSize, int range, int pageSize, bool refresh)
        {
            CheckDisposed();
            NativeApi.ScrollBar_SetScrollbar_(NativePointer, position, thumbSize, range, pageSize, refresh);
        }
        
        static GCHandle eventCallbackGCHandle;
        public static ScrollBar? GlobalObject;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.ScrollBarEventCallbackType((obj, e, parameter) =>
                    UI.Application.HandleThreadExceptions(() =>
                    {
                        var w = NativeObject.GetFromNativePointer<ScrollBar>(obj, p => new ScrollBar(p));
                        w ??= GlobalObject;
                        if (w == null) return IntPtr.Zero;
                        return w.OnEvent(e, parameter);
                    }
                    )
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.ScrollBar_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.ScrollBarEvent e, IntPtr parameter)
        {
            Scroll?.Invoke(); return IntPtr.Zero;
        }
        
        public Action? Scroll;
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr ScrollBarEventCallbackType(IntPtr obj, ScrollBarEvent e, IntPtr param);
            
            public enum ScrollBarEvent
            {
                Scroll,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ScrollBar_SetEventCallback_(ScrollBarEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ScrollBar_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ScrollBar_GetEventTypeID_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ScrollBar_GetEventOldPos_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ScrollBar_GetEventNewPos_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool ScrollBar_GetIsVertical_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ScrollBar_SetIsVertical_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ScrollBar_GetThumbPosition_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ScrollBar_SetThumbPosition_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ScrollBar_GetRange_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ScrollBar_GetThumbSize_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ScrollBar_GetPageSize_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ScrollBar_SetScrollbar_(IntPtr obj, int position, int thumbSize, int range, int pageSize, bool refresh);
            
        }
    }
}
