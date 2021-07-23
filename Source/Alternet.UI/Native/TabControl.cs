// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class TabControl : Control
    {
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
        
        public System.Drawing.SizeF GetTotalPreferredSizeFromPageSize(System.Drawing.SizeF pageSize)
        {
            CheckDisposed();
            return NativeApi.TabControl_GetTotalPreferredSizeFromPageSize_(NativePointer, pageSize);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr TabControl_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TabControl_GetPageCount_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TabControl_InsertPage_(IntPtr obj, int index, IntPtr page, string title);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TabControl_RemovePage_(IntPtr obj, int index, IntPtr page);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.SizeF TabControl_GetTotalPreferredSizeFromPageSize_(IntPtr obj, NativeApiTypes.SizeF pageSize);
            
        }
    }
}
