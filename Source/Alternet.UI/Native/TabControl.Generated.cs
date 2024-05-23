// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class TabControl : Control
    {
        static TabControl()
        {
            SetEventCallback();
        }
        
        public TabControl()
        {
            SetNativePointer(NativeApi.TabControl_Create_());
        }
        
        public TabControl(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public int PageCount
        {
            get
            {
                CheckDisposed();
                return NativeApi.TabControl_GetPageCount_(NativePointer);
            }
            
        }
        
        public int SelectedPageIndex
        {
            get
            {
                CheckDisposed();
                return NativeApi.TabControl_GetSelectedPageIndex_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TabControl_SetSelectedPageIndex_(NativePointer, value);
            }
        }
        
        public Alternet.UI.TabAlignment TabAlignment
        {
            get
            {
                CheckDisposed();
                return NativeApi.TabControl_GetTabAlignment_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TabControl_SetTabAlignment_(NativePointer, value);
            }
        }
        
        public void InsertPage(int index, Control page, string title)
        {
            CheckDisposed();
            NativeApi.TabControl_InsertPage_(NativePointer, index, page.NativePointer, title);
        }
        
        public void RemovePage(int index, Control page)
        {
            CheckDisposed();
            NativeApi.TabControl_RemovePage_(NativePointer, index, page.NativePointer);
        }
        
        public void SetPageTitle(int index, string title)
        {
            CheckDisposed();
            NativeApi.TabControl_SetPageTitle_(NativePointer, index, title);
        }
        
        public Alternet.Drawing.SizeD GetTotalPreferredSizeFromPageSize(Alternet.Drawing.SizeD pageSize)
        {
            CheckDisposed();
            return NativeApi.TabControl_GetTotalPreferredSizeFromPageSize_(NativePointer, pageSize);
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.TabControlEventCallbackType((obj, e, parameter) =>
                    UI.Application.HandleThreadExceptions(() =>
                    {
                        var w = NativeObject.GetFromNativePointer<TabControl>(obj, p => new TabControl(p));
                        if (w == null) return IntPtr.Zero;
                        return w.OnEvent(e, parameter);
                    }
                    )
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.TabControl_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.TabControlEvent e, IntPtr parameter)
        {
            switch (e)
            {
                case NativeApi.TabControlEvent.SelectedPageIndexChanged:
                {
                    SelectedPageIndexChanged?.Invoke(); return IntPtr.Zero;
                }
                case NativeApi.TabControlEvent.SelectedPageIndexChanging:
                {
                    var ea = new NativeEventArgs<TabPageSelectionEventData>(MarshalEx.PtrToStructure<TabPageSelectionEventData>(parameter));
                    SelectedPageIndexChanging?.Invoke(this, ea); return ea.Result;
                }
                default: throw new Exception("Unexpected TabControlEvent value: " + e);
            }
        }
        
        public Action? SelectedPageIndexChanged;
        public event NativeEventHandler<TabPageSelectionEventData>? SelectedPageIndexChanging;
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr TabControlEventCallbackType(IntPtr obj, TabControlEvent e, IntPtr param);
            
            public enum TabControlEvent
            {
                SelectedPageIndexChanged,
                SelectedPageIndexChanging,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TabControl_SetEventCallback_(TabControlEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr TabControl_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TabControl_GetPageCount_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TabControl_GetSelectedPageIndex_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TabControl_SetSelectedPageIndex_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.UI.TabAlignment TabControl_GetTabAlignment_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TabControl_SetTabAlignment_(IntPtr obj, Alternet.UI.TabAlignment value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TabControl_InsertPage_(IntPtr obj, int index, IntPtr page, string title);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TabControl_RemovePage_(IntPtr obj, int index, IntPtr page);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TabControl_SetPageTitle_(IntPtr obj, int index, string title);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.SizeD TabControl_GetTotalPreferredSizeFromPageSize_(IntPtr obj, Alternet.Drawing.SizeD pageSize);
            
        }
    }
}
